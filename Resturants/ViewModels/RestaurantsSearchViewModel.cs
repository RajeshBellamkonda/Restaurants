using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Restaurants.ViewModels
{
    public class RestaurantsSearchViewModel
    {
        [Required]
        public string PostCode { get; set; }
        public List<ResturantViewModel> Restaurants { get; set; }
        public string ErrorMessage => DisplayError ? $"No results found for postcode {PostCode}" : "";
        public bool DisplayError => !string.IsNullOrEmpty(PostCode) && (Restaurants == null || !Restaurants.Any());
    }
}
