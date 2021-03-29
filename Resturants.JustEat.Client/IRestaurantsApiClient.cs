using Restaurants.JustEat.Client.Models;
using System.Threading.Tasks;

namespace Restaurants.JustEat.Client
{
    public interface IRestaurantsApiClient
    {
        Task<RestaurantsByPostCode> GetRestaurantsByPostCodeAsync(string postcode);
    }
}