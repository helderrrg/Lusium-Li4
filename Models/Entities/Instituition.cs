namespace Models.Entities
{
    public class Instituition
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public required string NIF { get; set; }
        public int NumeroDeAssociacao { get; set; }
        public required string Email { get; set; }
        public required string Morada { get; set; }
        public int Creditos { get; set; }
        public required string PalavraPasse { get; set; }
    }
}