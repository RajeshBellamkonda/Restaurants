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

        public async Task<RestaurantSearchResultsDto> GetRestaurantsByPostCode(string postcode, int page, int pageSize)
        {
            {
                try
                {
                    var cleanPostCode = CleanString(postcode);
                    _cache.TryGetValue(cleanPostCode, out RestaurantsRoot restaurantsRoot);
                    if (restaurantsRoot == null)
                    {
                        restaurantsRoot = await _restaurantsApiClient.GetRestaurantsByPostCode(cleanPostCode);
                        _cache.Set(cleanPostCode, restaurantsRoot, TimeSpan.FromMinutes(_cacheSettings.ExpiryInMinutes));
                    }
                    if (restaurantsRoot != null)
                    {
                        return BuildRestaurantSearchResultsDto(page, pageSize, restaurantsRoot);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("ServiceError", ex, "Error occured while getting resturants by postcode");
                }
                return default;
            }
        }

        public async Task<RestaurantSearchResultsDto> GetRestaurantsByGeoLocation(string latitude, string longitude, int page, int pageSize)
        {
            try
            {
                var cleanLatitude = CleanString(latitude);
                var cleanLongitude = CleanString(longitude);
                var cacheKey = $"{latitude}_{longitude}";
                _cache.TryGetValue(cacheKey, out RestaurantsRoot restaurantsRoot);
                if (restaurantsRoot == null)
                {
                    restaurantsRoot = await _restaurantsApiClient.GetRestaurantsByLatLong(latitude, longitude);
                    _cache.Set(cacheKey, restaurantsRoot, TimeSpan.FromMinutes(_cacheSettings.ExpiryInMinutes));
                    // Adding the postcode to cache so that we get the same results for the postcode search too.
                    _cache.Set(CleanString(restaurantsRoot.MetaData.Postcode), restaurantsRoot, TimeSpan.FromMinutes(_cacheSettings.ExpiryInMinutes));
                }
                if (restaurantsRoot != null)
                {
                    return BuildRestaurantSearchResultsDto(page, pageSize, restaurantsRoot);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ServiceError", ex, "Error occured while getting resturants by geolocation");
            }
            return default;
        }

        private string CleanString(string stringToClean)
        {
            return stringToClean.Trim().ToLower();
        }

        private RestaurantSearchResultsDto BuildRestaurantSearchResultsDto(int page, int pageSize, RestaurantsRoot restaurantsByPostCode)
        {
            var resultRestaurants = restaurantsByPostCode.Restaurants
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new RestaurantSearchResultsDto
            {
                Restaurants = new StaticPagedList<RestaurantDto>(_mapper.Map<List<RestaurantDto>>(resultRestaurants),
                page, pageSize, restaurantsByPostCode.MetaData.ResultCount),
                PostCode = restaurantsByPostCode.MetaData.Postcode,
                Latitude = restaurantsByPostCode.MetaData.Latitude.ToString(),
                Longitude = restaurantsByPostCode.MetaData.Longitude.ToString()
            };
        }
    }
}
