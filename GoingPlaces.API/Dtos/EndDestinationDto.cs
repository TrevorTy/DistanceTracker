namespace GoingPlaces.API.Dtos
{
    public class EndDestinationDto
    {
        public string Name { get; set; }
        //To easily convert and roundup the Distance with ToString()
        public string Distance { get; set; }
        public int DistanceInt { get; set; }
        public int Year { get; set;}
        public string UserId { get; set; }
    }
}