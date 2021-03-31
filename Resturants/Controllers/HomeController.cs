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

        public async Task<IActionResult> Index(RestaurantsSearchViewModel restaurantSearchVm)
        {
            if (Request.Method == "GET")
            {
                ModelState.Clear();
                if (!string.IsNullOrEmpty(restaurantSearchVm.PostCodeFromQuery))
                {
                    var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(restaurantSearchVm.PostCodeFromQuery, restaurantSearchVm.Page, restaurantSearchVm.PageSize);
                    restaurantSearchVm = BuildViewModel(restaurantSearchResultsDto);
                    return View(restaurantSearchVm);
                }
                else if (!string.IsNullOrEmpty(restaurantSearchVm.Latitude) && !string.IsNullOrEmpty(restaurantSearchVm.Longitude))
                {
                    var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByGeoLocation(restaurantSearchVm.Latitude, restaurantSearchVm.Longitude, restaurantSearchVm.Page, restaurantSearchVm.PageSize);
                    restaurantSearchVm = BuildViewModel(restaurantSearchResultsDto);
                    return View(restaurantSearchVm);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(restaurantSearchVm.PostCode, restaurantSearchVm.Page, restaurantSearchVm.PageSize);
                    restaurantSearchVm = BuildViewModel(restaurantSearchResultsDto);
                    return View(restaurantSearchVm);
                }
                return View(restaurantSearchVm);
            }
            return View(new RestaurantsSearchViewModel());
        }

        private RestaurantsSearchViewModel BuildViewModel(RestaurantSearchResultsDto restaurantSearchResultsDto)
        {
            var restaurantsSearchViewModel = _mapper.Map<RestaurantsSearchViewModel>(restaurantSearchResultsDto);
            restaurantsSearchViewModel.Restaurants = MapPagedRestaurantDto(restaurantSearchResultsDto?.Restaurants, restaurantsSearchViewModel.Page, restaurantsSearchViewModel.PageSize);
            return restaurantsSearchViewModel;
        }


        private IPagedList<RestaurantViewModel> MapPagedRestaurantDto(IPagedList<RestaurantDto> pagedRestaurant, int pageNumber, int pageSize)
        {
            if (pagedRestaurant == null) return default;
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
