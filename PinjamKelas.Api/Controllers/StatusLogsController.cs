using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;
using PinjamKelas.Api.Dtos;

namespace PinjamKelas.Api.Controllers
{

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
                    .Select(s => new StatusLogDto
                    {
                        Id = s.Id,
                        IdClassroom = s.IdClassroom,
                        Description = s.Description,
                        LogTime = s.LogTime,
                        ClassroomName = s.Classroom != null ? s.Classroom.ClassName : "Unknown"
                    })
                    .ToListAsync();

                return Ok(new 
                { 
                    totalCount = statusLogs.Count,
                    data = statusLogs 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}