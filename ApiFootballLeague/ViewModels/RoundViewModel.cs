namespace ApiFootballLeague.ViewModels
{
    public class RoundViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public bool IsClosed { get; set; }
    }
}
