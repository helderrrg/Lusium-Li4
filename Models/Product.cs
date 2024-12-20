namespace Models
{
    public class Product
    {
        public int Id { get; set; }      // Chave primária
        public required string Name { get; set; } // Nome do produto
        public decimal Price { get; set; } // Preço do produto
    }
}