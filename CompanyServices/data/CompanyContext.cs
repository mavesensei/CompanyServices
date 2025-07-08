using Microsoft.EntityFrameworkCore;

namespace CompanyServices.Models
{
    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions options) : base(options) { }

        public DbSet<Companies> Companies { get; set; }
    }
}