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
        private const string ServiceError = "ServiceError";
        private const string GeoLocationErrorMessage = "Error occured while getting resturants by geolocation";
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
            var restaurantSearchResultsDto = new RestaurantSearchResultsDto();
            try
            {
                var cleanPostCode = CleanString(postcode);
                _cache.TryGetValue(cleanPostCode, out RestaurantsSearchResults restaurantsSearchResults);
                if (restaurantsSearchResults == null)
                {
                    var response = await _restaurantsApiClient.GetRestaurantsByPostCode(cleanPostCode);
                    var hasNoResults = !response.IsSuccess
                        || response.RestaurantsSearchResults?.Restaurants == null
                        || !response.RestaurantsSearchResults.Restaurants.Any();
                    if (hasNoResults)
                    {
                        restaurantSearchResultsDto.ErrorMessage = $"No resturants found for postcode: {postcode}";
                    }
                    else
                    {
                        restaurantsSearchResults = response.RestaurantsSearchResults;
                        _cache.Set(cleanPostCode, restaurantsSearchResults, TimeSpan.FromMinutes(_cacheSettings.ExpiryInMinutes));
                    }
                }
                restaurantSearchResultsDto = BuildRestaurantSearchResultsDto(page, pageSize, restaurantsSearchResults, restaurantSearchResultsDto);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error occured while getting resturants by postcode: {postcode}";
                _logger.LogError(ServiceError, ex, errorMessage);
                restaurantSearchResultsDto.ErrorMessage = errorMessage;
            }
            return restaurantSearchResultsDto;
        }

        public async Task<RestaurantSearchResultsDto> GetRestaurantsByGeoLocation(string latitude, string longitude, int page, int pageSize)
        {
            var restaurantSearchResultsDto = new RestaurantSearchResultsDto();
            try
            {
                var cleanLatitude = CleanString(latitude);
                var cleanLongitude = CleanString(longitude);
                var cacheKey = $"{cleanLatitude}_{cleanLongitude}";
                _cache.TryGetValue(cacheKey, out RestaurantsSearchResults restaurantsSearchResults);
                if (restaurantsSearchResults == null)
                {
                    var response = await _restaurantsApiClient.GetRestaurantsByLatLong(cleanLatitude, cleanLongitude);
                    var hasNoResults = !response.IsSuccess
                        || response.RestaurantsSearchResults?.Restaurants == null
                        || !response.RestaurantsSearchResults.Restaurants.Any();
                    if (hasNoResults)
                    {
                        restaurantSearchResultsDto.ErrorMessage = "No resturants found for this geolocation";
                    }
                    else
                    {
                        restaurantsSearchResults = response.RestaurantsSearchResults;
                        _cache.Set(cacheKey, restaurantsSearchResults, TimeSpan.FromMinutes(_cacheSettings.ExpiryInMinutes));
                        // Adding the postcode to cache so that we get the same results for the postcode search too.
                        _cache.Set(CleanString(restaurantsSearchResults.MetaData.Postcode), restaurantsSearchResults, TimeSpan.FromMinutes(_cacheSettings.ExpiryInMinutes));
                    }
                }
                restaurantSearchResultsDto = BuildRestaurantSearchResultsDto(page, pageSize, restaurantsSearchResults, restaurantSearchResultsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ServiceError, ex, GeoLocationErrorMessage);
                restaurantSearchResultsDto.ErrorMessage = GeoLocationErrorMessage;
            }
            return restaurantSearchResultsDto;
        }

        private string CleanString(string stringToClean) => stringToClean.Trim().ToLower();

        private RestaurantSearchResultsDto BuildRestaurantSearchResultsDto(int page, int pageSize, RestaurantsSearchResults restaurantsSearchResults, RestaurantSearchResultsDto restaurantSearchResultsDto)
        {
            var restaurants = restaurantsSearchResults?.Restaurants;
            if (restaurants != null && restaurants.Any())
            {
                var resultRestaurants = restaurantsSearchResults.Restaurants
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                restaurantSearchResultsDto.Restaurants = new StaticPagedList<RestaurantDto>(
                    _mapper.Map<List<RestaurantDto>>(resultRestaurants),
                    page, pageSize, restaurantsSearchResults.MetaData.ResultCount);

                restaurantSearchResultsDto.PostCode = restaurantsSearchResults.MetaData.Postcode;
                restaurantSearchResultsDto.Latitude = restaurantsSearchResults.MetaData.Latitude.ToString();
                restaurantSearchResultsDto.Longitude = restaurantsSearchResults.MetaData.Longitude.ToString();
            }
            return restaurantSearchResultsDto;
        }
    }
}
