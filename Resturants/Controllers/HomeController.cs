using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using Restaurants.JustEat.Client;
using Restaurants.Services;
using Restaurants.Services.DTOs;
using Restaurants.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRestaurantsApiClient _restaurantsApiClient;
        private readonly IMapper _mapper;
        private readonly IRestaurantsService _restaurantsService;

        public HomeController(ILogger<HomeController> logger, IRestaurantsApiClient restaurantsApiClient, IMapper mapper, IRestaurantsService restaurantsService)
        {
            _logger = logger;
            _restaurantsApiClient = restaurantsApiClient;
            _mapper = mapper;
            _restaurantsService = restaurantsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string query, int? page)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var restaurantsSearchViewModel = await GetRestaurantVm(query, page.HasValue ? page.Value : 1);
                return View(restaurantsSearchViewModel);
            }
            return View(new RestaurantsSearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(RestaurantsSearchViewModel restaurantSearchVm)
        {
            if (ModelState.IsValid)
            {
                restaurantSearchVm = await GetRestaurantVm(restaurantSearchVm.PostCode, restaurantSearchVm.Page);
            }
            return View(restaurantSearchVm);
        }

        private async Task<RestaurantsSearchViewModel> GetRestaurantVm(string query, int page)
        {
            var restaurantSearchVm = new RestaurantsSearchViewModel { PostCode = query, Page = page };
            var restaurantsByPostCode = await _restaurantsService.GetRestaurantsByPostCode(restaurantSearchVm.PostCode, restaurantSearchVm.Page, restaurantSearchVm.PageSize);
            restaurantSearchVm.Restaurants = MapPagedRestaurantDto(restaurantsByPostCode, restaurantSearchVm.Page, restaurantSearchVm.PageSize);
            return restaurantSearchVm;
        }

        private IPagedList<RestaurantViewModel> MapPagedRestaurantDto(IPagedList<RestaurantDto> pagedRestaurant, int pageNumber, int pageSize)
        {
            var mappedResults = _mapper.Map<IEnumerable<RestaurantViewModel>>(pagedRestaurant);

            return new StaticPagedList<RestaurantViewModel>(mappedResults, pagedRestaurant.GetMetaData());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
