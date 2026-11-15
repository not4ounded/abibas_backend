using abibas_back.DTOs;
using abibas_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace abibas_back.Controllers
{
    [ApiController]
    [Route("auth/[Controller]")] // /auth/values
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// м/д проверки юзера в бд
        /// </summary>
        /// <param name="dto">DTO с почтой</param>
        /// <returns></returns>
        [HttpPost("register/init")]
        public async Task<IActionResult> UserInitRegister([FromBody] UserInitRegisterDTO dto)
        {
            bool EmailFound = await _context.UserItems.AnyAsync(e => e.Email == dto.Email);

            if (EmailFound)
            {
                return BadRequest("Пользователь с такой почтой уже зарегистрирован");
            }

            await GenerateAndSendCode(dto.Email);
            return Ok(new { message = "Код отправлен" });
        }

        /// <summary>
        /// повторная отправка 6-ти знач. кода 
        /// </summary>
        /// <param name="dto">DTO с почтой</param>
        /// <returns></returns>
        [HttpPost("register/resend")]
        public async Task<IActionResult> CodeResend([FromBody] UserInitRegisterDTO dto)
        {
            bool EmailFound = await _context.UserItems.AnyAsync(e => e.Email == dto.Email);

            if (EmailFound)
            {
                return BadRequest("Пользователь с такой почтой уже зарегистрирован");
            }

            await GenerateAndSendCode(dto.Email);
            return Ok(new { message = "Код отправлен повторно" });
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
            var verificationCode = Random.Shared.Next(100000, 1000000).ToString();
            var createdAt = DateTime.UtcNow;

            var verificationCodeItem = new VerificationCodeItem
            {
                Email = email,
                Code = verificationCode,
                CreatedAt = createdAt,
                ExpiresAt = createdAt.AddMinutes(10)
            };

            _context.VerificationCodeItems.Add(verificationCodeItem);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[Email MOGG] Код верификации для {email}: {verificationCode}");
        }
    }
}
