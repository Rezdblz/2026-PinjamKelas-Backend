using System;

namespace PinjamKelas.Api.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int IdUsers { get; set; }
        public int IdClassroom { get; set; }
        public string? Description { get; set; }
        public required PostStatus Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        
        public User? User { get; set; }
        public Classroom? Classroom { get; set; }
    }

    public enum PostStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed
    }
}