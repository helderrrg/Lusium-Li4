namespace Models.Entities
{
    public interface IUser {
        public int ID { get; set; }
        public string Nome { get; set; }

        public string Email { get; set; }
    }
}