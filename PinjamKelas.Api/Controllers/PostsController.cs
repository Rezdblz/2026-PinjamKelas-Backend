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
                var posts = await _context.Posts.ToListAsync();
                return Ok(posts);
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
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
                if (post == null)
                    return NotFoundResponse("Post not found");

                return Ok(post);
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
                    .ToListAsync();

                if (!posts.Any())
                    return NotFoundResponse("No posts found for this user");

                return Ok(posts);
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
                    Status = Enum.Parse<PostStatus>(dto.Status),
                    CreatedAt = DateTime.UtcNow,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
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

                if (!string.IsNullOrWhiteSpace(dto.Status))
                    post.Status = Enum.Parse<PostStatus>(dto.Status);

                if (dto.StartTime.HasValue)
                    post.StartTime = dto.StartTime.Value;

                if (dto.EndTime.HasValue)
                    post.EndTime = dto.EndTime.Value;

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                return SuccessResponse(post, "Post updated successfully");
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
    }
}