namespace ApiFootballLeague.ViewModels
{
    public class PlayerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;     
        public string? ClubName { get; set; }
        public string? PositionName{ get; set; }
        public string? ImageUrl { get; set; }
    }
}
