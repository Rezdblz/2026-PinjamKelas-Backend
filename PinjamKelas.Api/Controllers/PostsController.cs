using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;
using PinjamKelas.Api.Dtos;

namespace PinjamKelas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : BaseController
    {
        public PostsController(AppDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Classroom)
                    .ToListAsync();

                if (!posts.Any())
                    return NotFoundResponse("No posts found");

                var postDtos = posts.Select(p => MapToDto(p)).ToList();
                return Ok(postDtos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Classroom)
                    .FirstOrDefaultAsync(p => p.Id == id);
                
                if (post == null)
                    return NotFoundResponse("Post not found");

                var postDto = MapToDto(post);
                return Ok(postDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            try
            {
                var posts = await _context.Posts
                    .Where(p => p.IdUsers == userId)
                    .Include(p => p.User)
                    .Include(p => p.Classroom)
                    .ToListAsync();

                if (!posts.Any())
                    return NotFoundResponse("No posts found for this user");

                var postDtos = posts.Select(p => MapToDto(p)).ToList();
                return Ok(postDtos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return ValidationErrorResponse("Title is required");

                var post = new Post
                {
                    Title = dto.Title,
                    IdUsers = dto.IdUsers,
                    IdClassroom = dto.IdClassroom,
                    Description = dto.Description,
                    Status = (PostStatus)dto.Status,
                    CreatedAt = DateTime.UtcNow,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                var createdPost = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Classroom)
                    .FirstOrDefaultAsync(p => p.Id == post.Id);
                
                if (createdPost == null)
                    return BadRequest("Failed to retrieve created post");

                var postDto = MapToDto(createdPost);
                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, postDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto dto)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
                if (post == null)
                    return NotFoundResponse("Post not found");

                if (!string.IsNullOrWhiteSpace(dto.Title))
                    post.Title = dto.Title;

                if (!string.IsNullOrWhiteSpace(dto.Description))
                    post.Description = dto.Description;

                if (dto.Status.HasValue)
                    post.Status = (PostStatus)dto.Status.Value;

                if (dto.StartTime.HasValue)
                    post.StartTime = dto.StartTime.Value;

                if (dto.EndTime.HasValue)
                    post.EndTime = dto.EndTime.Value;

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                var updatedPost = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Classroom)
                    .FirstOrDefaultAsync(p => p.Id == id);
                
                if (updatedPost == null)
                    return BadRequest("Failed to retrieve updated post");

                var postDto = MapToDto(updatedPost);
                return SuccessResponse(postDto, "Post updated successfully");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _context.Posts.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (post == null)
                    return NotFoundResponse("Post not found");

                post.DeletedAt = DateTime.UtcNow;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                return SuccessResponse(null, "Post deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private PostDto MapToDto(Post post)
        {
            return new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                IdUsers = post.IdUsers,
                IdClassroom = post.IdClassroom,
                Description = post.Description,
                Status = (int)post.Status,
                CreatedAt = post.CreatedAt,
                StartTime = post.StartTime,
                EndTime = post.EndTime,
                User = post.User != null ? new UserDto 
                { 
                    Id = post.User.Id, 
                    Username = post.User.Username,
                    Role = post.User.Role
                } : null,
                Classroom = post.Classroom != null ? new ClassroomDto 
                { 
                    Id = post.Classroom.Id, 
                    ClassName = post.Classroom.ClassName
                } : null
            };
        }
    }
}