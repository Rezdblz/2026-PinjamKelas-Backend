using System;

namespace PinjamKelas.Api.Models
{
    public class StatusLog : BaseEntity
    {
        public int IdClassroom { get; set; }
        public string? Description { get; set; }
        public DateTime LogTime { get; set; }

        public Classroom? Classroom { get; set; }
    }
}