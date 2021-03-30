using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        public async Task<RestaurantsRoot> GetRestaurantsByPostCodeAsync(string postcode)
        {
            try
            {
                var result = await _httpClient.GetAsync($"restaurants/bypostcode/{postcode}");
                if (result.IsSuccessStatusCode)
                {
                    string restaurantsByPostCodeJson = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RestaurantsRoot>(restaurantsByPostCodeJson, new JsonSerializerSettings
                    {
                        Error = (object sender, ErrorEventArgs args) =>
                        {
                            _logger.LogError("JustEatApiClientDeserializeError", args.ErrorContext.Error.Message);
                            args.ErrorContext.Handled = true;
                        },
                    });
                }
                else
                {
                    _logger.LogError("JustEatApiClientError", $"StatusCode:{result.StatusCode}", await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("JustEatApiClientError", ex, "Error occured while getting resturants by postcode");
            }
            return default;
        }
        public async Task<RestaurantsRoot> GetRestaurantsByLatLong(string latitude, string longitude)
        {
            try
            {
                var result = await _httpClient.GetAsync($"restaurants/bylatlong?latitude={latitude}&longitude={longitude}");
                if (result.IsSuccessStatusCode)
                {
                    string restaurantsByLatLong = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RestaurantsRoot>(restaurantsByLatLong, new JsonSerializerSettings
                    {
                        Error = (object sender, ErrorEventArgs args) =>
                        {
                            _logger.LogError("JustEatApiClientDeserializeError", args.ErrorContext.Error.Message);
                            args.ErrorContext.Handled = true;
                        },
                    });
                }
                else
                {
                    _logger.LogError("JustEatApiClientError", $"StatusCode:{result.StatusCode}", await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("JustEatApiClientError", ex, "Error occured while getting resturants by postcode");
            }
            return default;
        }
    }
}
