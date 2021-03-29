using System.Collections.Generic;

namespace Restaurants.ViewModels
{
    public class RestaurantsSearchViewModel
    {
        public string PostCode { get; set; }
        public List<ResturantViewModel> Resturants { get; set; }
        public string ErrorMessage { get; set; }
        public bool DisplayError { get; set; }

    }
}
