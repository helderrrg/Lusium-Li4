namespace Models.Entities
{
    public class Page
    {
        public int ID { get; set; }
        public required string ImagemAlusiva { get; set; }
        public int Numeracao { get; set; }
        public int ManualAssociado { get; set; }
    }
}