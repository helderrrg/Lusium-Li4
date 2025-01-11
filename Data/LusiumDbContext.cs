using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data
{
    public class LusiumDbContext : DbContext
    {
        public LusiumDbContext(DbContextOptions<LusiumDbContext> options) : base(options) { }

        public required DbSet<Product> Produto { get; set; }

        public required DbSet<Purchase> Compra { get; set; }

        public required DbSet<Manual> Manual { get; set; }

        public required DbSet<Page> Pagina { get; set; }

        public required DbSet<Administrator> Administrador { get; set; }
        
        public required DbSet<Instituition> Instituicao { get; set; }

        public required DbSet<Collaborator> Colaborador { get; set; }

        public required DbSet<Piece> Peca { get; set; }

        public required DbSet<InstituitionManual> ManualDaInstituicao { get; set; }

        public required DbSet<PiecePerProduct> PecaPorProduto { get; set; }
    }
}
