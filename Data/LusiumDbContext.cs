using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data
{
    public class LusiumDbContext : DbContext
    {
        public LusiumDbContext(DbContextOptions<LusiumDbContext> options) : base(options) { }

        public required DbSet<Product> Produto { get; set; }
    }
}
