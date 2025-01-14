namespace Models.Entities
{
    public class Administrator : IUser
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string PalavraPasse { get; set; }
    }
}