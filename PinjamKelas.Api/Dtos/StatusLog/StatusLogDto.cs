namespace PinjamKelas.Api.Dtos
{
    public class StatusLogDto
    {
        public int Id { get; set; }
        public int IdClassroom { get; set; }
        public string? Description { get; set; }
        public DateTime LogTime { get; set; }
        public string? ClassroomName { get; set; }
    }
}