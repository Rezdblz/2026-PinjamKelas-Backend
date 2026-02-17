using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;
using PinjamKelas.Api.Dtos;

namespace PinjamKelas.Api.Controllers
{
    public class ClassroomsController : BaseController
    {
        public ClassroomsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/classrooms
        [HttpGet]
        public async Task<IActionResult> GetClassrooms()
        {
            try
            {
                var classrooms = await _context.Classrooms.ToListAsync();
                return Ok(classrooms);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/classrooms/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassroom(int id)
        {
            try
            {
                var classroom = await _context.Classrooms.FirstOrDefaultAsync(c => c.Id == id);
                if (classroom == null)
                    return NotFoundResponse("Classroom not found");

                return Ok(classroom);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // POST: api/classrooms
        [HttpPost]
        public async Task<IActionResult> CreateClassroom([FromBody] CreateClassroomDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.ClassName))
                    return ValidationErrorResponse("ClassName is required");

                var classroom = new Classroom
                {
                    ClassName = dto.ClassName,
                    Status = ClassroomStatus.Available
                };

                _context.Classrooms.Add(classroom);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetClassroom), new { id = classroom.Id }, classroom);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // PUT: api/classrooms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassroom(int id, [FromBody] UpdateClassroomDto dto)
        {
            try
            {
                var classroom = await _context.Classrooms.FirstOrDefaultAsync(c => c.Id == id);
                if (classroom == null)
                    return NotFoundResponse("Classroom not found");

                if (!string.IsNullOrWhiteSpace(dto.ClassName))
                    classroom.ClassName = dto.ClassName;

                if (dto.Status.HasValue)
                    classroom.Status = dto.Status.Value;

                _context.Classrooms.Update(classroom);
                await _context.SaveChangesAsync();

                return SuccessResponse(classroom, "Classroom updated successfully");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE: api/classrooms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            try
            {
                var classroom = await _context.Classrooms.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (classroom == null)
                    return NotFoundResponse("Classroom not found");

                classroom.DeletedAt = DateTime.UtcNow;
                _context.Classrooms.Update(classroom);
                await _context.SaveChangesAsync();

                return SuccessResponse(null, "Classroom deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/classrooms/{id}/status-logs
        [HttpGet("{id}/status-logs")]
        public async Task<IActionResult> GetClassroomStatusLogs(int id)
        {
            try
            {
                var classroom = await _context.Classrooms.FirstOrDefaultAsync(c => c.Id == id);
                if (classroom == null)
                    return NotFoundResponse("Classroom not found");

                var statusLogs = await _context.StatusLogs
                    .Where(sl => sl.IdClassroom == id)
                    .ToListAsync();

                return Ok(statusLogs);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}