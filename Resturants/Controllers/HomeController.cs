using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurants.Services;
using Restaurants.Services.DTOs;
using Restaurants.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Restaurants.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IRestaurantsService _restaurantsService;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IRestaurantsService restaurantsService)
        {
            _logger = logger;
            _mapper = mapper;
            _restaurantsService = restaurantsService;
        }

        /// <summary>
        /// Displays restaurants search view
        /// </summary>
        /// <param name="postcode">postcode to search</param>
        /// <param name="latitude">latitude from geolocation to search</param>
        /// <param name="longitude">longitude from geolocation to search</param>
        /// <param name="page">The page search results to retrieve</param>
        /// <returns>View for the Restaurants Search</returns>
        [HttpGet]
        public async Task<IActionResult> Index(string postcode = null, string latitude = null, string longitude = null, int page = 1)
        {
            if (!string.IsNullOrEmpty(postcode))
            {
                var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(postcode, page, RestaurantsSearchViewModel.DefaultPageSize);
                var restaurantSearchVm = BuildViewModel(restaurantSearchResultsDto);
                return View(restaurantSearchVm);
            }
            else if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByGeoLocation(latitude, longitude, page, RestaurantsSearchViewModel.DefaultPageSize);
                var restaurantSearchVm = BuildViewModel(restaurantSearchResultsDto);
                return View(restaurantSearchVm);
            }
            return View(new RestaurantsSearchViewModel());
        }

        /// <summary>
        /// Displays Restaurant search results based on PostCode and GeoLocation
        /// </summary>
        /// <param name="restaurantSearchVm">Restaurant Search View Model with search parameters</param>
        /// <returns>View for Restaurants Search Results</returns>
        [HttpPost]
        public async Task<IActionResult> Index(RestaurantsSearchViewModel restaurantSearchVm)
        {
            if (ModelState.IsValid)
            {
                var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(restaurantSearchVm.Postcode, restaurantSearchVm.Page, restaurantSearchVm.PageSize);
                restaurantSearchVm = BuildViewModel(restaurantSearchResultsDto);
                return View(restaurantSearchVm);
            }
            return View(restaurantSearchVm);
        }

        private RestaurantsSearchViewModel BuildViewModel(RestaurantSearchResultsDto restaurantSearchResultsDto)
        {
            var restaurantsSearchViewModel = _mapper.Map<RestaurantsSearchViewModel>(restaurantSearchResultsDto);
            return restaurantsSearchViewModel;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
