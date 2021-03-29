namespace Restaurants.ViewModels
{
    public class ResturantViewModel
    {

        public int Id { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public float RatingStars { get; set; }
        public int NumberOfRatings { get; set; }
        public float RatingAverage { get; set; }
        public CuisineViewModel[] Cuisines { get; set; }
    }
}
