namespace StripsRest.DTOs
{
    public class StripDto
    {
        public string Url { get; set; }
        public string Titel { get; set; }
        public int Nr { get; set; }
        public string? Reeks { get; set; }
        public string? ReeksUrl { get; set; }
        public string Uitgeverij { get; set; }
        public string UitgeverijUrl { get; set; }
        public List<AuteurDto> Auteurs { get; set; } = new();
    }
}
