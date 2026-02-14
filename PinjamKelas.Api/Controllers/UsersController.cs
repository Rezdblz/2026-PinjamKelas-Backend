using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;
using PinjamKelas.Api.Dtos;
namespace PinjamKelas.Api.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(AppDbContext context):base(context)
        {
            
        
        }
        //Get:api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex) 
            {
                return HandleException(ex);
            }
        }
        //Get:api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user= await _context.Users.FirstOrDefaultAsync(u=> u.Id == id);
                if (user == null)
                    return NotFoundResponse("user not found");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        //to:do add create update delete user
        //POST: api/users
        
    }
}