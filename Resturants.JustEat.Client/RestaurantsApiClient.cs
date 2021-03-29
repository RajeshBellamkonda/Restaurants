using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Restaurants.JustEat.Client.Models;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Restaurants.JustEat.Client
{
    public class RestaurantsApiClient : IRestaurantsApiClient
    {
        private readonly IRestClient _restClient;

        public RestaurantsApiClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<RestaurantsByPostCode> GetRestaurantsByPostCodeAsync(string postcode)
        {
            try
            {
                //return await _restClient.GetAsync<RestaurantsByPostCode>(new RestRequest($"https://uk.api.just-eat.io/restaurants/bypostcode/{postcode}", Method.GET));
                using var httpClient = new HttpClient();
                var result = await httpClient.GetAsync($"https://uk.api.just-eat.io/restaurants/bypostcode/{postcode}");
                if (result.IsSuccessStatusCode)
                {
                    string restaurantsByPostCodeJson = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RestaurantsByPostCode>(restaurantsByPostCodeJson, new JsonSerializerSettings
                    {
                        Error = (object sender, ErrorEventArgs args) =>
                        {
                            args.ErrorContext.Handled = true;
                        },
                    });
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return default;
        }
    }
}
