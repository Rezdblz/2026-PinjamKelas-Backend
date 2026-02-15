using System;
using System.Linq;
using PinjamKelas.Api.Models;
using BCrypt.Net;

namespace PinjamKelas.Api.DbSeeder
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any())
                return;

            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin"
                },
                new User
                {
                    Username = "teacher",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("teacher123"),
                    Role = "Teacher"
                },
                new User
                {
                    Username = "student",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                    Role = "Student"
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed classrooms
            if (!context.Classrooms.Any())
            {
                context.Classrooms.AddRange(
                    new Classroom 
                    { 
                        ClassName = "A101", 
                        Status = ClassroomStatus.Available 
                    },
                    new Classroom 
                    { 
                        ClassName = "B202", 
                        Status = ClassroomStatus.Unavailable 
                    },
                    new Classroom 
                    { 
                        ClassName = "C303", 
                        Status = ClassroomStatus.Maintenance 
                    }
                );
                context.SaveChanges();
            }

            // Seed posts
            var user = context.Users.FirstOrDefault();
            var classroom = context.Classrooms.FirstOrDefault();

            if (user != null && classroom != null && !context.Posts.Any())
            {
                context.Posts.Add(new Post
                {
                    Title = "Sample Classroom Booking",
                    IdUsers = user.Id,
                    IdClassroom = classroom.Id,
                    Description = "This is a test booking for the classroom.",
                    Status = PostStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    StartTime = DateTime.UtcNow.AddHours(1),
                    EndTime = DateTime.UtcNow.AddHours(3)
                });
                context.SaveChanges();
            }

            // StatusLog is seeded by database trigger, not here
        }
    }
}