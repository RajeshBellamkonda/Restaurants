using Restaurants.JustEat.Client.Models;
using System.Threading.Tasks;

namespace Restaurants.JustEat.Client
{
    public interface IRestaurantsApiClient
    {
        Task<RestaurantsClientResponse> GetRestaurantsByLatLong(string latitude, string longitude);
        Task<RestaurantsClientResponse> GetRestaurantsByPostCode(string postcode);
    }
}