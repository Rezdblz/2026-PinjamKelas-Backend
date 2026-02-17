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

        /// <summary>
        /// Handles exceptions and returns a standardized error response
        /// </summary>
        protected IActionResult HandleException(Exception ex)
        {
            return BadRequest(new 
            { 
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Returns a success response
        /// </summary>
        protected IActionResult SuccessResponse(object? data = null, string message = "Success")
        {
            return Ok(new 
            { 
                message,
                data
            });
        }

        /// <summary>
        /// Returns a not found response
        /// </summary>
        protected IActionResult NotFoundResponse(string message = "Resource not found")
        {
            return NotFound(new { message });
        }

        /// <summary>
        /// Returns a validation error response
        /// </summary>
        protected IActionResult ValidationErrorResponse(string message)
        {
            return BadRequest(new { message });
        }
    }
}