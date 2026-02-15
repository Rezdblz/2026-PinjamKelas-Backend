using System;

namespace PinjamKelas.Api.Models
{
    public class Post : BaseEntity
    {
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
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3
    }
}