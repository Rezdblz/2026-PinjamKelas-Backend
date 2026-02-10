using Microsoft.EntityFrameworkCore;
using PinjamKelas.Api.Models;

namespace PinjamKelas.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<StatusLog> StatusLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User-Post
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.IdUsers);

            // Classroom-Post
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Classroom)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.IdClassroom);

            // Classroom-StatusLog
            modelBuilder.Entity<StatusLog>()
                .HasOne(s => s.Classroom)
                .WithMany(c => c.StatusLogs)
                .HasForeignKey(s => s.IdClassroom);
        }
    }
}