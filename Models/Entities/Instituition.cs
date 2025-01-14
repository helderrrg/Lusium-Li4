namespace Models.Entities
{
    public class Instituition : IUser
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public required string NIF { get; set; }
        public int NumeroAssociacao { get; set; }
        public required string Email { get; set; }
        public required string Morada { get; set; }
        public int Creditos { get; set; }
        public required string PalavraPasse { get; set; }
        public ICollection<InstituitionManual>? ManualInstituicoes { get; set; }
        public ICollection<Purchase>? Compras { get; set; }
    }
}