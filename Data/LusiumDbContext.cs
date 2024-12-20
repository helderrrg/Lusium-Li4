using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class LusiumDbContext : DbContext
    {
        public LusiumDbContext(DbContextOptions<LusiumDbContext> options) : base(options) { }

        public required DbSet<Product> Products { get; set; } //Have required keyword to make sure that the DbSet is not null
    }
}
