using abibas_back.DTOs;
using abibas_back.JWT.Interfaces;
using abibas_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace abibas_back.Controllers
{
    /// <summary>
    /// Контроллер авторизации
    /// </summary>
    [ApiController]
    [Route("auth")] // /auth/values
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
        /// эндпоинт регистрации (1 шаг)
        /// </summary>
        /// <param name="dto">DTO с полями пользователя</param>
        /// <returns></returns>
        [HttpPost("register/init")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> UserInitRegister([FromBody] UserInitRegisterDTO dto)
        {
            if (dto is null)
                return BadRequest("Некорректные данные. Необходимо заполнить все обязательные поля");
            
            var item = await _context.UserItems.FirstOrDefaultAsync(i => i.Email == dto.Email);

            if (item is not null)
                return Ok("Если почта не занята, то код отправлен");

            if (dto.Email is not null)
                await GenerateAndSendCode(dto.Email);
            else
                return BadRequest("Некорректные данные. Необходимо заполнить все обязательные поля");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var pendingRegItem = new PendingRegistrationItem
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNum = dto.PhoneNum,
                PasswordHash = hashedPassword,
                AgreeToTerms = dto.AgreeToTerms
            };

            var oldPending = await _context.PendingRegistrationsItems.Where(p => p.Email == dto.Email).ToListAsync();
            _context.PendingRegistrationsItems.RemoveRange(oldPending);
            _context.PendingRegistrationsItems.Add(pendingRegItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Если почта не занята, то код отправлен" });
        }

        /// <summary>
        /// эндпоинт повторной отправки 6-ти знач. кода 
        /// </summary>
        /// <param name="dto">DTO с почтой</param>
        /// <returns></returns>
        [HttpPost("register/resend")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> CodeResend([FromBody] UserResetDTO dto)
        {
            bool EmailFound = await _context.UserItems.AnyAsync(e => e.Email == dto.Email);

            if (!EmailFound)
                await GenerateAndSendCode(dto.Email);
         
            return Ok(new { message = "Если почта не занята, то код отправлен" });
        }

        /// <summary>
        /// эндпоинт регистрации (2 шаг)
        /// </summary>
        /// <param name="dto">DTO с данными пользователя</param>
        /// <returns></returns>
        [HttpPost("register/complete")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> UserCompleteRegister([FromBody] UserCompleteRegisterDTO dto)
        {
            var otp = await _context.VerificationCodeItems.Where(x => x.Email == dto.Email && !x.IsInvalidated && x.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (otp is null)
                return NotFound("Пользователь не найден");

            var isCodeVerified = await VerifyOTPAsync(otp, dto.VerificationCode);

            if (!isCodeVerified || await _context.UserItems.AnyAsync(e => e.Email == dto.Email))
                return BadRequest("Неверный код верификации");

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var pendingItem = await _context.PendingRegistrationsItems.FirstOrDefaultAsync(i => i.Email == dto.Email);

                if (pendingItem is null)
                    return BadRequest("Заявка устарела, повторите регистрацию заново");

                var userItem = new UserItem
                {
                    FirstName = pendingItem.FirstName,
                    LastName = pendingItem.LastName,
                    Email = pendingItem.Email,
                    PhoneNum = pendingItem.PhoneNum,
                    PasswordHash = pendingItem.PasswordHash,
                    AgreeToTerms = pendingItem.AgreeToTerms
                };

                _context.PendingRegistrationsItems.Remove(pendingItem);
                _context.UserItems.Add(userItem);
                await _context.SaveChangesAsync();

                string tokenString = _jwtProvider.GenerateAccessToken(userItem);
                var refreshToken = _jwtProvider.GenerateRefreshToken(userItem);

                _context.VerificationCodeItems.Remove(otp);
                _context.RefreshTokenItems.Add(refreshToken.Value);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { AccessTokenResponse = tokenString, RefreshToken = refreshToken.Key, message = "Пользователь успешно зарегистрирован" });
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// эндпоинт логина 
        /// </summary>
        /// <param name="dto">логин DTO</param>
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
        /// эндпоинт 
        /// </summary>
        /// <param name="dto">DTO с почтой</param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        [EnableRateLimiting("IpRateLimit")]
        public async Task<IActionResult> SendResetPasswordURL([FromBody] UserResetDTO dto)
        {
            var user = await _context.UserItems.FirstOrDefaultAsync(i => i.Email == dto.Email);

            if (user is null)
                return Ok("Если аккаунт с указаной почтой существует, мы отправили на него инструкцию по смене пароля. \n" +
                    "Проверьте входящие и папку \"Спам\". (консоль)");

            // генерация токена для URL (смены пароля)
            string rawToken = RandomNumberGenerator.GetHexString(64);

            // хеширование токена
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            string tokenHash = Convert.ToHexString(hashBytes);

            var resetToken = new PasswordResetTokenItem
            {
                UserId = user.Id,
                User = user,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            };

            _context.PasswordResetTokenItems.Add(resetToken);
            await _context.SaveChangesAsync();

            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string resetUrl = $"{baseUrl}/auth/reset-password?token={Uri.EscapeDataString(rawToken)}";

            Console.WriteLine($"[Email MOGG] Ссылка для сброса пароля: {resetUrl}");

            return Ok("Если аккаунт с указаной почтой существует, мы отправили на него инструкцию по смене пароля. \n" +
                    "Проверьте входящие и папку \"Спам\". (консоль)");
        }

        /// <summary>
        /// эндпоинт смены пароля
        /// </summary>
        /// <param name="dto">DTO со сменой пароля (с валидацией)</param>
        /// <returns></returns>
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(dto.ResetToken));
            string tokenHash = Convert.ToHexString(hashBytes);
            
            var resetToken = await _context.PasswordResetTokenItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

            if (resetToken is null || resetToken.ExpiresAt < DateTime.UtcNow)
                return BadRequest("Недействительный или просроченный токен сброса пароля");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            resetToken.User.PasswordHash = hashedPassword;

            RemoveOldRefreshTokens(resetToken.User);

            string tokenString = _jwtProvider.GenerateAccessToken(resetToken.User);
            var refreshToken = _jwtProvider.GenerateRefreshToken(resetToken.User);

            _context.RefreshTokenItems.Add(refreshToken.Value);

            _context.PasswordResetTokenItems.Remove(resetToken);
            await _context.SaveChangesAsync();

            return Ok(new { AccessTokenResponse = tokenString, refreshToken = refreshToken.Key, message = "Пароль успешно изменен"});
        }

        /// <summary>
        /// ротация рефреш токена
        /// </summary>
        /// <param name="dto">DTO рефреш токена</param>
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
        /// выход из аккаунта (обнуление рефреша)
        /// </summary>
        /// <param name="dto">DTO рефреш токена</param>
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
            var oldCodes = await _context.VerificationCodeItems.Where(v => v.Email == email).ToListAsync();
            _context.VerificationCodeItems.RemoveRange(oldCodes);
            var verificationCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var hashedCode = BCrypt.Net.BCrypt.HashPassword(verificationCode);
            var createdAt = DateTime.UtcNow;

            var verificationCodeItem = new VerificationCodeItem
            {
                Email = email,
                CodeHash = hashedCode,
                ExpiresAt = createdAt.AddMinutes(10),
                IsInvalidated = false
            };

            _context.VerificationCodeItems.Add(verificationCodeItem);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[Email MOGG] Код верификации для {email}: {verificationCode}. Статус: [{verificationCodeItem.IsInvalidated}]");
        }

        /// <summary>
        /// асинхронный м/д для верификации и проверки otp-кода на совпадение
        /// </summary>
        /// <param name="otp">итем 6-ти значного кода</param>
        /// <param name="inputCode">код введенный пользователем</param>
        /// <returns></returns>
        private async Task<bool> VerifyOTPAsync(VerificationCodeItem otp, string inputCode)
        {
            const int maxAttempts = 5;

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
        /// м/д удаления невалидных рефреш токенов
        /// </summary>
        /// <param name="user">итем пользователя</param>
        private void RemoveOldRefreshTokens(UserItem user)
        {
            var expired = _context.RefreshTokenItems.Where(t => t.UserId == user.Id && (t.IsRevoked || t.ExpirationTime < DateTime.UtcNow));
            _context.RefreshTokenItems.RemoveRange(expired);
        }
    }
}
