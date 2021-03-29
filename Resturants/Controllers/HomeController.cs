using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurants.JustEat.Client;
using Restaurants.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Restaurants.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRestaurantsApiClient _restaurantsApiClient;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IRestaurantsApiClient restaurantsApiClient, IMapper mapper)
        {
            _logger = logger;
            _restaurantsApiClient = restaurantsApiClient;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(RestaurantsSearchViewModel restaurantSearchVm)
        {
            if (!string.IsNullOrEmpty(restaurantSearchVm.PostCode))
            {
                var restaurantsByPostCode = await _restaurantsApiClient.GetRestaurantsByPostCodeAsync(restaurantSearchVm.PostCode);
                if (restaurantsByPostCode != null)
                {
                    restaurantSearchVm.Resturants = _mapper.Map<List<ResturantViewModel>>(restaurantsByPostCode.Restaurants);
                }
                else
                {
                    restaurantSearchVm.ErrorMessage = $"No results found for postcode {restaurantSearchVm.PostCode}";
                    restaurantSearchVm.DisplayError = true;
                }
            }
            return View(restaurantSearchVm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
