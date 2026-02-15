namespace PinjamKelas.Api.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int IdUsers { get; set; }
        public int IdClassroom { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}