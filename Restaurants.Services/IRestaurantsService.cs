using PagedList.Core;
using Restaurants.Services.DTOs;
using System.Threading.Tasks;

namespace Restaurants.Services
{
    public interface IRestaurantsService
    {
        Task<RestaurantSearchResultsDto> GetRestaurantsByGeoLocation(string latitude, string longitude, int page, int pageSize);
        Task<RestaurantSearchResultsDto> GetRestaurantsByPostCode(string postcode, int page, int pageSize);

    }
}