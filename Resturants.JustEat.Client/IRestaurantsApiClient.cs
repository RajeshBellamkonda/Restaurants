using Restaurants.JustEat.Client.Models;
using System.Threading.Tasks;

namespace Restaurants.JustEat.Client
{
    public interface IRestaurantsApiClient
    {
        Task<RestaurantsSearchResults> GetRestaurantsByLatLong(string latitude, string longitude);
        Task<RestaurantsSearchResults> GetRestaurantsByPostCode(string postcode);
    }
}