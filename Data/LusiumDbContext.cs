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

        public required DbSet<InstituitionManual> ManualInstituicao { get; set; }

        public required DbSet<PiecePerProduct> PecaProduto { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Config Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ID);

            // Config Purchase
            modelBuilder.Entity<Purchase>()
                .HasKey(p => p.NumeroCompra);
            
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Produto)
                .WithMany(p => p.Compras)
                .HasForeignKey(p => p.ProdutoAssociado);
            
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Instituicao)
                .WithMany(i => i.Compras)
                .HasForeignKey(p => p.InstituicaoID);

            // Config Manual
            modelBuilder.Entity<Manual>()
                .HasKey(m => m.ID);
            
            modelBuilder.Entity<Manual>()
                .HasMany(m => m.Paginas);

            // Config Page
            modelBuilder.Entity<Page>()
                .HasKey(p => p.ID);

            // Config Administrator
            modelBuilder.Entity<Administrator>()
                .HasKey(a => a.ID);

            // Config Instituition
            modelBuilder.Entity<Instituition>()
                .HasKey(i => i.ID);
            
            modelBuilder.Entity<Instituition>()
                .HasMany(i => i.Colaboradores);

            // Config Collaborator
            modelBuilder.Entity<Collaborator>()
                .HasKey(c => c.ID);

            modelBuilder.Entity<Collaborator>()
                .HasOne(c => c.Instituicao)
                .WithMany(i => i.Colaboradores)
                .HasForeignKey(c => c.InstituicaoID);

            // Config InstitutionManual
            modelBuilder.Entity<InstituitionManual>()
                .HasKey(im => new { im.IDInstituicao, im.IDManual });

            modelBuilder.Entity<InstituitionManual>()
                .HasOne(im => im.Instituicao)
                .WithMany(i => i.ManualInstituicoes)
                .HasForeignKey(im => im.IDInstituicao);

            modelBuilder.Entity<InstituitionManual>()
                .HasOne(im => im.Manual)
                .WithMany(m => m.ManualInstituicoes)
                .HasForeignKey(im => im.IDManual);

            // Config PiecePerProduct
            modelBuilder.Entity<PiecePerProduct>()
                .HasKey(pp => new { pp.IDPeca, pp.IDProduto });

            modelBuilder.Entity<PiecePerProduct>()
                .HasOne(pp => pp.Peca)
                .WithMany(p => p.PecaProdutos)
                .HasForeignKey(pp => pp.IDPeca);

            modelBuilder.Entity<PiecePerProduct>()
                .HasOne(pp => pp.Produto)
                .WithMany(p => p.PecaProdutos)
                .HasForeignKey(pp => pp.IDProduto);

            base.OnModelCreating(modelBuilder);
        }
    }
}
