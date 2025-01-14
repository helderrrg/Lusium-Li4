namespace Models.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public int Custo { get; set; }
        public int IdadeMinima { get; set; }
        public required string ImagemAlusiva { get; set; }
        public int ManualAssociado { get; set; }
        public ICollection<PiecePerProduct>? PecaProdutos { get; set; }
        public ICollection<Purchase>? Compras { get; set; }
    }
}