namespace Models.Entities
{
    public class Purchase
    {
        public int NumeroDaCompra { get; set; }
        public DateTime DataDeCompra { get; set; }
        public required string EnderecoDeEntrega { get; set; }
        public int ProdutoAssociado { get; set; }
        public int Instituicao { get; set; }
    }
}