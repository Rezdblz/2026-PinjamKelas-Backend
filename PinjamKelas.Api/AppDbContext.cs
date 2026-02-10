using Microsoft.EntityFrameworkCore;

namespace PinjamKelas.Api
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add your DbSet properties here, e.g.:
        // public DbSet<User> Users { get; set; }
    }
}