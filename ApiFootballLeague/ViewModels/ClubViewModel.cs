namespace ApiFootballLeague.ViewModels
{
    public class ClubViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }      

        public string? Stadium { get; set; } 

        public int Capacity { get; set; }

        public string? HeadCoach { get; set; }
        public string? ImageFullPath { get; set; }   
    }
}
