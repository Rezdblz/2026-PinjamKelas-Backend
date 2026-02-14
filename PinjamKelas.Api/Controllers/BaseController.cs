using Microsoft.AspNetCore.Mvc;
using PinjamKelas.Api.Models;

namespace PinjamKelas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

        protected IActionResult Handleexception(Exception ex)
        {
            return BadRequest(new{message=ex.Message});
        }
    }
}