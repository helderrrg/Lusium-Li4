namespace Models.Entities
{
    public class InstituitionManual
    {
        public int IDInstituicao { get; set; }
        public Instituition? Instituicao { get; set; }

        public int IDManual { get; set; }
        public Manual? Manual { get; set; }
    }
}
