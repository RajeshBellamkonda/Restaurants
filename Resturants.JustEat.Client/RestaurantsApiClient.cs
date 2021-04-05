using Newtonsoft.Json;
using Restaurants.JustEat.Client.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

        public async Task<RestaurantsSearchResults> GetRestaurantsByPostCode(string postcode)
        {
            _logger.LogDebug($"GetRestaurantsByPostCode for postcode {postcode}");
            var result = await _httpClient.GetAsync($"restaurants/bypostcode/{postcode}");
            result.EnsureSuccessStatusCode();
            _logger.LogDebug($"GetRestaurantsByPostCode for postcode success for {postcode}");
            string restaurantsByPostCodeJson = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RestaurantsSearchResults>(restaurantsByPostCodeJson);
        }

        public async Task<RestaurantsSearchResults> GetRestaurantsByLatLong(string latitude, string longitude)
        {
            _logger.LogDebug($"GetRestaurantsByLatLong for latitude:{latitude} & longitude:{longitude}");
            var result = await _httpClient.GetAsync($"restaurants/bylatlong?latitude={latitude}&longitude={longitude}");
            result.EnsureSuccessStatusCode();
            _logger.LogDebug($"GetRestaurantsByLatLong success for latitude:{latitude} & longitude:{longitude}");
            string restaurantsByLatLong = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RestaurantsSearchResults>(restaurantsByLatLong);
        }
    }
}
