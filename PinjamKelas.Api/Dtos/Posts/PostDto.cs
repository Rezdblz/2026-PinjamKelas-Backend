namespace PinjamKelas.Api.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int IdUsers { get; set; }
        public int IdClassroom { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public UserDto? User { get; set; }
        public ClassroomDto? Classroom { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public required string Role { get; set; }
    }

    public class ClassroomDto
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
    }
}