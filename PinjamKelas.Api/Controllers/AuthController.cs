using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;
using PinjamKelas.Api.Dtos;
using BCrypt.Net;

namespace PinjamKelas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == dto.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                    return Unauthorized("Invalid credentials");

                return Ok(new { id = user.Id, username = user.Username, role = user.Role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}