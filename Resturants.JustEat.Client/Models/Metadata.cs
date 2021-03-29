namespace Restaurants.JustEat.Client.Models
{
    public class Metadata
    {
        public string CanonicalName { get; set; }
        public string District { get; set; }
        public string Postcode { get; set; }
        public string Area { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Cuisinedetail[] CuisineDetails { get; set; }
        public int ResultCount { get; set; }
        public object SearchedTerms { get; set; }
        public Tagdetail[] TagDetails { get; set; }
        public object CollectionExperimentInjectedRestaurantIds { get; set; }
        public string DeliveryArea { get; set; }
    }

}
