namespace PinjamKelas.Api.Dtos
{
    public class CreatePostDto
    {
        public required string Title { get; set; }
        public int IdUsers { get; set; }
        public int IdClassroom { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}