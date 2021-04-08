using System.Collections.Generic;
using System.Text;

namespace Restaurants.JustEat.Client.Models
{

    public class RestaurantsSearchResults
    {
        public Metadata MetaData { get; set; }
        public List<Restaurant> Restaurants { get; set; }
    }

}
