using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Restaurants.ViewModels
{
    public class RestaurantsSearchViewModel
    {
        [Required]
        public string PostCode { get; set; }

        [FromQuery(Name = "postcode")]
        public string PostCodeFromQuery { get; set; }

        [FromQuery(Name = "latitude")]
        public string Latitude { get; set; }

        [FromQuery(Name = "longitude")]
        public string Longitude { get; set; }

        public IPagedList<RestaurantViewModel> Restaurants { get; set; }

        public string ErrorMessage => DisplayError ? $"No results found for postcode {PostCode}" : "";
        public bool DisplayError => !string.IsNullOrEmpty(PostCode) && !HasResults;
        public bool HasResults => Restaurants != null && Restaurants.Any();
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;


    }
}
