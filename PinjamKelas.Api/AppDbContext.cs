using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PinjamKelas.Api.Models
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
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Classroom
            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("classroom");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClassName).IsRequired();
                entity.Property(e => e.Status).IsRequired();
            });

            // Post
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(e => e.IdUsers);

                entity.HasOne(e => e.Classroom)
                      .WithMany(c => c.Posts)
                      .HasForeignKey(e => e.IdClassroom);
            });

            // StatusLog
            modelBuilder.Entity<StatusLog>(entity =>
            {
                entity.ToTable("status_log");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description);
                entity.Property(e => e.LogTime).IsRequired();

                entity.HasOne(e => e.Classroom)
                      .WithMany(c => c.StatusLogs)
                      .HasForeignKey(e => e.IdClassroom);
            });

            // Configure soft delete globally
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var parameter = Expression.Parameter(entity.ClrType);
                var deletedProperty = entity.FindProperty("DeletedAt");
                
                if (deletedProperty != null)
                {
                    var filter = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, deletedProperty.PropertyInfo!),
                            Expression.Constant(null)
                        ),
                        parameter
                    );
                    
                    entity.SetQueryFilter(filter);
                }
            }
        }
    }
}