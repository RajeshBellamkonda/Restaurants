namespace Restaurants.JustEat.Client.Models
{
    public class Restaurantset
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Restaurant[] Restaurants { get; set; }
    }

}
