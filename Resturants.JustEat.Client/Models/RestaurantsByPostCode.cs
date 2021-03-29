using System.Collections.Generic;
using System.Text;

namespace Restaurants.JustEat.Client.Models
{

    public class RestaurantsByPostCode
    {
        public string Area { get; set; }
        public Metadata MetaData { get; set; }
        public Restaurant[] Restaurants { get; set; }
        public object[] Dishes { get; set; }
        public string ShortResultText { get; set; }
        public Promotedplacement promotedPlacement { get; set; }
    }

}
