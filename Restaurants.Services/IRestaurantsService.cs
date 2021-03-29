using PagedList.Core;
using Restaurants.Services.DTOs;
using System.Threading.Tasks;

namespace Restaurants.Services
{
    public interface IRestaurantsService
    {

        public Task<IPagedList<RestaurantDto>> GetRestaurantsByPostCode(string postcode, int page, int pageSize);
        
    }
}