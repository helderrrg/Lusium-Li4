using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class LusiumService
    {
        private readonly LusiumDbContext _context;

        public LusiumService(LusiumDbContext context)
        {
            _context = context;
        }

        // Método para obter todos os produtos
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Método para adicionar um novo produto
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Método para atualizar um produto
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Método para excluir um produto
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}