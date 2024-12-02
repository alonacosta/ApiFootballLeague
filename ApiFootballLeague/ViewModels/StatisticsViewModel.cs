namespace ApiFootballLeague.ViewModels
{
    public class StatisticsViewModel
    {
        public int Position { get; set; }
        public string? ClubName { get; set; }
        public Guid ImageId { get; set; }
        public string? ImageFullPath { get; set; }
        public int TotalMatches { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int Points { get; set; }
        public int Finished { get; set; }
        public int Scheduled { get; set; }
    }
}
