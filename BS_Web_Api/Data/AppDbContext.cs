using BS_Web_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BS_Web_Api.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost; Database=TestDB; User Id=sa; Password=c21484; TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }


    }
}
