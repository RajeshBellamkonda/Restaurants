using PagedList.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Restaurants.ViewModels
{
    public class RestaurantsSearchViewModel
    {
        [Required]
        public string PostCode { get; set; }
        public List<RestaurantViewModel> RestaurantsOld { get; set; }
        public IPagedList<RestaurantViewModel> Restaurants { get; set; }

        public string ErrorMessage => DisplayError ? $"No results found for postcode {PostCode}" : "";
        public bool DisplayError => !string.IsNullOrEmpty(PostCode) && !HasResults;
        public bool HasResults => Restaurants != null && Restaurants.Any();
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
