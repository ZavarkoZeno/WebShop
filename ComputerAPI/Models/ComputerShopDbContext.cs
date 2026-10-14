using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Models
{
    public class ComputerShopDbContext : DbContext
    {
        public ComputerShopDbContext(DbContextOptions options) : base(options)
        {
        }

        public ComputerShopDbContext()
        {
        }

        public DbSet<Osystem> Osystems { get; set; }
        public DbSet<Computer> Computers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=computer;user=root;password=", ServerVersion.AutoDetect("server=localhost;database=computer;user=root;password="));
        }
    }
}
