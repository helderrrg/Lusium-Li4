namespace Models.Entities
{
    public class Piece
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public required string ImagemAlusiva { get; set; }
        public int Quantidade { get; set; }
        public ICollection<PiecePerProduct>? PecaProdutos { get; set; }
    }
}