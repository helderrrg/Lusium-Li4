namespace Models.Entities
{
    public class PiecePerProduct
    {
        public int IDPeca { get; set; }
        public Piece? Peca { get; set; }
        public int IDProduto { get; set; }
        public Product? Produto { get; set; }
        public int Quantidade { get; set; }
    }
}