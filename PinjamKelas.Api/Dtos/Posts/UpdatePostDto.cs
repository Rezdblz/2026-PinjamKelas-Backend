namespace PinjamKelas.Api.Dtos
{
    public class UpdatePostDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}