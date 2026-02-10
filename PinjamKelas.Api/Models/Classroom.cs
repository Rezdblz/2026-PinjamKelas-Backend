using System.Collections.Generic;

namespace PinjamKelas.Api.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Status { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<StatusLog> StatusLogs { get; set; }
    }
}