namespace Models.Entities
{
    public class Manual
    {
        public int ID { get; set; }
        public required string Capa { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public ICollection<InstituitionManual>? ManualInstituicoes { get; set; }
        public ICollection<Page>? Paginas { get; set; }
    }
}