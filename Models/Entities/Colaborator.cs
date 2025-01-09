namespace Models.Entities
{
    public class Colaborator
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public DateOnly DataDeNascimento { get; set; }
        public int Instituicao { get; set; }
        public required string PalavraPasse { get; set; }
    }
}