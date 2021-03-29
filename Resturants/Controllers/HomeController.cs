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

        [HttpGet]
        public IActionResult Index() { return View(new RestaurantsSearchViewModel()); }

        [HttpPost]
        public async Task<IActionResult> Index(RestaurantsSearchViewModel restaurantSearchVm)
        {
            if (ModelState.IsValid)
            {
                var restaurantsByPostCode = await _restaurantsApiClient.GetRestaurantsByPostCodeAsync(restaurantSearchVm.PostCode);
                if (restaurantsByPostCode != null)
                {
                    restaurantSearchVm.Restaurants = _mapper.Map<List<ResturantViewModel>>(restaurantsByPostCode.Restaurants);
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
