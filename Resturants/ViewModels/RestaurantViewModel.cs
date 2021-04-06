using System.ComponentModel.DataAnnotations;

namespace Restaurants.ViewModels
{
    public class RestaurantViewModel
    {
        public int Id { get; set; }

        public string LogoUrl { get; set; }
        public string Name { get; set; }

        [Display(Name = "Rating")]
        public float RatingStars { get; set; }

        [Display(Name = "Number of Ratings")]
        public int NumberOfRatings { get; set; }
        
        [Display(Name = "Average Rating")]
        public float RatingAverage { get; set; }
        public CuisineViewModel[] Cuisines { get; set; }
    }
}
