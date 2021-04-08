using Newtonsoft.Json;
using Restaurants.JustEat.Client.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Restaurants.JustEat.Client
{
    public class RestaurantsApiClient : IRestaurantsApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IJustEatApiClientSettings _justEatApiClientSettings;
        private readonly ILogger<RestaurantsApiClient> _logger;

        public RestaurantsApiClient(HttpClient httpClient, IJustEatApiClientSettings justEatApiClientSettings, ILogger<RestaurantsApiClient> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(justEatApiClientSettings.BaseAddress);
            _justEatApiClientSettings = justEatApiClientSettings;
            _logger = logger;
        }

        public void Dispose()
        {
            this._httpClient?.Dispose();
        }

        public async Task<RestaurantsClientResponse> GetRestaurantsByPostCode(string postcode)
        {
            var response = new RestaurantsClientResponse();
            _logger.LogDebug($"GetRestaurantsByPostCode for postcode {postcode}");
            var result = await _httpClient.GetAsync($"restaurants/bypostcode/{postcode}");
            response.IsSuccess = result.StatusCode == HttpStatusCode.OK;
            if (response.IsSuccess)
            {
                _logger.LogDebug($"GetRestaurantsByPostCode for postcode success for {postcode}");
                string restaurantsByPostCodeJson = await result.Content.ReadAsStringAsync();
                response.RestaurantsSearchResults = JsonConvert.DeserializeObject<RestaurantsSearchResults>(restaurantsByPostCodeJson);
            }
            return response;
        }

        public async Task<RestaurantsClientResponse> GetRestaurantsByLatLong(string latitude, string longitude)
        {
            var response = new RestaurantsClientResponse();
            _logger.LogDebug($"GetRestaurantsByLatLong for latitude:{latitude} & longitude:{longitude}");
            var result = await _httpClient.GetAsync($"restaurants/bylatlong?latitude={latitude}&longitude={longitude}");
            response.IsSuccess = result.StatusCode == HttpStatusCode.OK;
            if (response.IsSuccess)
            {
                _logger.LogDebug($"GetRestaurantsByLatLong success for latitude:{latitude} & longitude:{longitude}");
                string restaurantsByLatLong = await result.Content.ReadAsStringAsync();
                response.RestaurantsSearchResults = JsonConvert.DeserializeObject<RestaurantsSearchResults>(restaurantsByLatLong);
            }
            return response;
        }
    }
}
