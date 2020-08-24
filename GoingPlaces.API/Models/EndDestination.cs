namespace GoingPlaces.API.Models
{
    public class EndDestination
    {
        public int Id { get; set; }
        public string Goal { get; set; }
        public int DistanceOfGoal { get; set; }
        public string UserId { get; set; }
        public int Year { get; set; }
        public User User { get; set; }
    }
}