using Models;
using Microsoft.EntityFrameworkCore;
using Data;

namespace Services
{
    public class LusiumService
    {
        private readonly LusiumDbContext _context;

        public LusiumService(LusiumDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Produto.ToListAsync();
        }
    }
}