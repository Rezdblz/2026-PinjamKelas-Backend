using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;
using PinjamKelas.Api.Dtos;

namespace PinjamKelas.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatusLogsController : BaseController
    {
        public StatusLogsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/statuslogs
        [HttpGet]
        public async Task<IActionResult> GetAllStatusLogs()
        {
            try
            {
                var statusLogs = await _context.StatusLogs
                    .Include(s => s.Classroom)
                    .OrderByDescending(s => s.LogTime)
                    .ToListAsync();

                return Ok(new 
                { 
                    totalCount = statusLogs.Count,
                    data = statusLogs 
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}