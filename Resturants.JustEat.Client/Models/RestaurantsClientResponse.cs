using System.Net;

namespace Restaurants.JustEat.Client.Models
{
    public class RestaurantsClientResponse
    {
        public RestaurantsSearchResults RestaurantsSearchResults { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

}
