namespace Models.Entities
{
    public class Purchase
    {
        public int NumeroCompra { get; set; }
        public DateOnly DataCompra { get; set; }
        public required string EnderecoEntrega { get; set; }
        public int ProdutoAssociado { get; set; }
        public Product? Produto { get; set; }
        public int InstituicaoID { get; set; }
        public Instituition? Instituicao { get; set; }
    }
}