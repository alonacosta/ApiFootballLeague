namespace ApiFootballLeague.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }        

        public string? HomeTeam { get; set; }

        public string? AwayTeam { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public bool IsClosed { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsFinished { get; set; }
    }
}
