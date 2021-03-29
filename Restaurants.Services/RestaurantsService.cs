using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using Restaurants.JustEat.Client;
using Restaurants.JustEat.Client.Models;
using Restaurants.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Services
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IRestaurantsApiClient _restaurantsApiClient;
        private readonly IMemoryCache _cache;
        private readonly ICacheSettings _cacheSettings;
        private readonly IMapper _mapper;

        public RestaurantsService(ILogger<RestaurantsService> logger, IRestaurantsApiClient restaurantsApiClient, IMemoryCache cache, ICacheSettings cacheSettings, IMapper mapper)
        {
            _logger = logger;
            _restaurantsApiClient = restaurantsApiClient;
            _cache = cache;
            _cacheSettings = cacheSettings;
            _mapper = mapper;
        }

        public async Task<IPagedList<RestaurantDto>> GetRestaurantsByPostCode(string postcode, int page, int pageSize)
        {
            {
                try
                {
                    var cleanPostCode = CleanPostCode(postcode);
                    _cache.TryGetValue(cleanPostCode, out RestaurantsByPostCode restaurantsByPostCode);
                    if (restaurantsByPostCode == null)
                    {
                        restaurantsByPostCode = await _restaurantsApiClient.GetRestaurantsByPostCodeAsync(cleanPostCode);
                        _cache.Set(cleanPostCode, restaurantsByPostCode, TimeSpan.FromMinutes(_cacheSettings.ExpiryInMinutes));
                    }
                    if (restaurantsByPostCode != null)
                    {
    
                        var resultRestaurants = restaurantsByPostCode.Restaurants
                            .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                        return new StaticPagedList<RestaurantDto>(_mapper.Map<List<RestaurantDto>>(resultRestaurants),
                            page, pageSize, restaurantsByPostCode.Restaurants.Count);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("ServiceError", ex, "Error occured while getting resturants by postcode");
                }
                return default;
            }
        }

        private string CleanPostCode(string postcode)
        {
            return postcode.Trim().ToLower();
        }
    }
}
