using System;

namespace PinjamKelas.Api.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int IdUsers { get; set; }
        public int IdClassroom { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Expiry { get; set; }

        public User User { get; set; }
        public Classroom Classroom { get; set; }
    }
}