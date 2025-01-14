namespace Models.Entities
{
    public class Collaborator : IUser
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public DateOnly DataNascimento { get; set; }
        public int InstituicaoID { get; set; }
        public required string PalavraPasse { get; set; }
    }
}