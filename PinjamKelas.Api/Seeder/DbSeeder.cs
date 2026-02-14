using System;
using System.Linq;
using PinjamKelas.Api.Models;

namespace PinjamKelas.Api.DbSeeder
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Seed test user
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Username = "testuser",
                    Role = "admin",
                    CreatedAt = DateTime.UtcNow
                });
                context.SaveChanges();
            }

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