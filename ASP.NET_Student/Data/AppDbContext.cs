using ASP.NET_Student.Models;
using Microsoft.EntityFrameworkCore;


namespace ASP.NET_Student.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }


    }
}
