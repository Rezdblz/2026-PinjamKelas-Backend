using PinjamKelas.Api.Models;

namespace PinjamKelas.Api.Dtos
{
    public class UpdateClassroomDto
    {
        public string? ClassName { get; set; }
        public ClassroomStatus? Status { get; set; }
    }
}