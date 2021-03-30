using Restaurants.JustEat.Client.Models;
using System.Threading.Tasks;

namespace Restaurants.JustEat.Client
{
    public interface IRestaurantsApiClient
    {
        Task<RestaurantsRoot> GetRestaurantsByLatLong(string latitude, string longitude);
        Task<RestaurantsRoot> GetRestaurantsByPostCodeAsync(string postcode);
    }
}