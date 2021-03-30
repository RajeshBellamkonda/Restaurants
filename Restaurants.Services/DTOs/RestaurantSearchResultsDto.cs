using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Services.DTOs
{
    public class RestaurantSearchResultsDto
    {
        public IPagedList<RestaurantDto> Restaurants { get; set; }
        
        public string PostCode { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
