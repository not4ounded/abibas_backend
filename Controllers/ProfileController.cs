using abibas_back.DTOs;
using abibas_back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace abibas_back.Controllers
{
    /// <summary>
    /// контроллер профиля
    /// </summary>
    [ApiController]
    [Route("profile")]
    public class ProfileController : ControllerBase
    {
        public readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// апдейт личной инф. юзера
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserPersonalInfoDTO dto)
        {
            var userTokenId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            long.TryParse(userTokenId, out long userId);

            if (userId == 0) 
                return NotFound("Пользователь не найден");

            if (await _context.UserItems.AnyAsync(u => u.Email == dto.Email && u.Id != userId))
                return BadRequest("Этот Email уже используется");
            
            var user = await _context.UserItems.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) return NotFound("Пользователь не найден");

            bool emailChanged = user.Email != dto.Email;
            
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNum = dto.PhoneNum;

            if (emailChanged)
            {
                user.Email = dto.Email;

                var token = await _context.RefreshTokenItems
                    .Where(u => u.UserId == userId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsRevoked, p => true));

                await _context.SaveChangesAsync();

                return Ok(new { message = "Данные успешно изменены. Пожалуйста, повторите вход", requiresLogin = emailChanged });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Данные успешно изменены."});
        }
    }
}
