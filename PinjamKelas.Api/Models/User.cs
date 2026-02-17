using System;
using System.Collections.Generic;

namespace PinjamKelas.Api.Models
{
    public class User : BaseEntity
    {
        public required string Username { get; set; }
        public required string PasswordHash {get;set;}
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Post>? Posts { get; set; }
    }
}