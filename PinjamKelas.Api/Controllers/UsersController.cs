using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;

namespace PinjamKelas.Api.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(AppDbContext context):base(context)
        {
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}