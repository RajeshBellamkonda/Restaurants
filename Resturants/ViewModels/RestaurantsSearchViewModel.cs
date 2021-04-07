using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Restaurants.ViewModels
{
    public class RestaurantsSearchViewModel
    {
        public const int DefaultPageSize = 10;

        [Required]
        public string PostCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public StaticPagedList<RestaurantViewModel> Restaurants { get; set; }
        public string ErrorMessage { get; set; }
        public bool DisplayError => !string.IsNullOrEmpty(ErrorMessage) && !HasResults;
        public bool HasResults => Restaurants != null && Restaurants.Any();
        public int PageSize { get; set; } = DefaultPageSize;
        public int Page { get; set; } = 1;


    }
}
