using abibas_back.DTOs;
using abibas_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Cryptography;
using abibas_back.JWT.Interfaces;
using System.Text;
using Microsoft.Build.Construction;

namespace abibas_back.Controllers
{
    [ApiController]
    [Route("auth/[Controller]")] // /auth/values
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJWTProvider _jwtProvider;

        public AuthController(AppDbContext context, IJWTProvider jwtProvider)
        {
            _context = context;
            _jwtProvider = jwtProvider;
        }

        /// <summary>
        /// м/д проверки юзера в бд
        /// </summary>
        /// <param name="dto">DTO с почтой</param>
        /// <returns></returns>
        [HttpPost("register/init")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> UserInitRegister([FromBody] UserInitRegisterDTO dto)
        {
            bool EmailFound = await _context.UserItems.AnyAsync(e => e.Email == dto.Email);

            if (!EmailFound)
            {
                await GenerateAndSendCode(dto.Email);
            }

            return Ok(new { message = "Если почта не занята, то код отправлен" });
        }

        /// <summary>
        /// повторная отправка 6-ти знач. кода 
        /// </summary>
        /// <param name="dto">DTO с почтой</param>
        /// <returns></returns>
        [HttpPost("register/resend")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> CodeResend([FromBody] UserInitRegisterDTO dto)
        {
            bool EmailFound = await _context.UserItems.AnyAsync(e => e.Email == dto.Email);

            if (!EmailFound)
            {
                await GenerateAndSendCode(dto.Email);
            }
         
            return Ok(new { message = "Если почта не занята, то код отправлен" });
        }


        /// <summary>
        /// Подтверждение личности пользователяы
        /// </summary>
        /// <param name="dto">DTO с данными пользователя</param>
        /// <returns></returns>
        [HttpPost("register/complete")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> UserCompleteRegister([FromBody] UserCompleteRegisterDTO dto)
        {
            if (await _context.UserItems.AnyAsync(e => e.Email == dto.Email))
                return BadRequest("Неверный код верификации");

            var item = await _context.VerificationCodeItems.OrderByDescending(i => i.Id).FirstOrDefaultAsync(i => i.Email == dto.Email);

            if (item is null)
            {
                return NotFound("Пользователь не найден");
            }

            var isCodeVerified = await VerifyOTPAsync(dto.Email, dto.VerificationCode);

            if (!isCodeVerified)
            {
                return BadRequest("Неверный код верификации");
            }

            if (DateTime.UtcNow > item.ExpiresAt)
            {
                return BadRequest("Код верификации истек");
            }

            // хеширование пароля 
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var userItem = new UserItem
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNum = dto.PhoneNum,
                PasswordHash = hashedPassword,
            };


            string tokenString = _jwtProvider.GenerateAccessToken(userItem);
            var refreshToken = _jwtProvider.GenerateRefreshToken(userItem);

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.UserItems.Add(userItem);
                _context.VerificationCodeItems.Remove(item);
                _context.RefreshTokenItems.Add(refreshToken.Value);


                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }

            await _context.SaveChangesAsync();

            return Ok(new { AccessTokenResponse = tokenString, RefreshToken = refreshToken.Key, message = "Пользователь успешно зарегистрирован" });
        }

        /// <summary>
        /// м/д логина пользователя
        /// </summary>
        /// <param name="dto">логин дто</param>
        /// <returns></returns>
        [HttpPost("login")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginDTO dto)
        {
            var userItem = await _context.UserItems.FirstOrDefaultAsync(i => i.Email == dto.Login || i.PhoneNum == dto.Login);

            if (userItem is null || !BCrypt.Net.BCrypt.Verify(dto.Password, userItem.PasswordHash))
                return Unauthorized("Неверное имя пользователя или пароль");

            RemoveOldRefreshTokens(userItem);

            string tokenString = _jwtProvider.GenerateAccessToken(userItem);
            var refreshToken = _jwtProvider.GenerateRefreshToken(userItem);

            _context.RefreshTokenItems.Add(refreshToken.Value);

            await _context.SaveChangesAsync();

            return Ok(new { AccessTokenResponse = tokenString, RefreshToken = refreshToken.Key, message = "Пользователь успешно авторизован" });
        }

        /// <summary>
        /// ротация рефреш токена
        /// </summary>
        /// <param name="dto">дто рефреш токена</param>
        /// <returns></returns>
        [HttpPost("refresh")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> UpdateRefreshToken([FromBody]RefreshTokenDTO dto)
        {
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Token)));
            var token = await _context.RefreshTokenItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TokenHash == hash);

            if (token is null || token.IsRevoked || token.ExpirationTime < DateTime.UtcNow)
            {
                return Unauthorized("Недействительный токен");
            }

            RemoveOldRefreshTokens(token.User);

            token.IsRevoked = true;

            var accessToken = _jwtProvider.GenerateAccessToken(token.User);
            var refreshToken = _jwtProvider.GenerateRefreshToken(token.User);

            _context.RefreshTokenItems.Add(refreshToken.Value);
            await _context.SaveChangesAsync();

            return Ok(new { AccessTokenResponse = accessToken, RefreshToken = refreshToken.Key });
        }

        /// <summary>
        /// выход из аккаунта
        /// </summary>
        /// <param name="dto">дто рефреш токена</param>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO dto)
        {
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Token)));
            var token = await _context.RefreshTokenItems.FirstOrDefaultAsync(t => t.TokenHash == hash);
            
            if (token is null) 
                return NoContent();
            else if (token is not null && !token.IsRevoked)
            {
                token.IsRevoked = true;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Ok();
        }

        /// <summary>
        /// м/д генерации 6-ти знач. кода
        /// </summary>
        /// <param name="email">почта</param>
        /// <returns></returns>
        private async Task GenerateAndSendCode(string email)
        {
            var oldCodes = _context.VerificationCodeItems.Where(v => v.Email == email);
            _context.VerificationCodeItems.RemoveRange(oldCodes);
            var verificationCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var hashedCode = BCrypt.Net.BCrypt.HashPassword(verificationCode);
            var createdAt = DateTime.UtcNow;

            var verificationCodeItem = new VerificationCodeItem
            {
                Email = email,
                CodeHash = hashedCode,
                ExpiresAt = createdAt.AddMinutes(10)
            };

            _context.VerificationCodeItems.Add(verificationCodeItem);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[Email MOGG] Код верификации для {email}: {verificationCode}");
        }

        /// <summary>
        /// м/д для верификации и проверки otp-кода на совпадение
        /// </summary>
        /// <param name="email">почта</param>
        /// <param name="inputCode">код</param>
        /// <returns></returns>
        private async Task<bool> VerifyOTPAsync(string email, string inputCode)
        {
            const int maxAttempts = 5;

            var otp = await _context.VerificationCodeItems.Where(x => x.Email == email && !x.IsInvalidated && x.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (otp is null)
                return false;

            if (otp.FailedAttempts >= maxAttempts)
                return false;

            if (otp.IsInvalidated == true)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(inputCode, otp.CodeHash)) 
            {
                otp.FailedAttempts++;
                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                otp.IsInvalidated = true;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// удаление невалидных рефреш токенов
        /// </summary>
        /// <param name="user">итем пользователя</param>
        private void RemoveOldRefreshTokens(UserItem user)
        {
            var expired = _context.RefreshTokenItems.Where(t => t.UserId == user.Id && (t.IsRevoked || t.ExpirationTime < DateTime.UtcNow));
            _context.RefreshTokenItems.RemoveRange(expired);
        }
    }
}
