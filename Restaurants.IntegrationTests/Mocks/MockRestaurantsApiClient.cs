using Restaurants.JustEat.Client;
using Restaurants.JustEat.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.IntegrationTests.Mocks
{
    public class MockRestaurantsApiClient : IRestaurantsApiClient
    {
        public MockRestaurantsApiClient()
        {

        }
        //return await Task.Run(() => GetMockPayment(id));
        public async Task<RestaurantsSearchResults> GetRestaurantsByLatLong(string latitude, string longitude)
        {
            return await Task.Run(() => GetMockedRestaurantsSearchResults());
        }

        public async Task<RestaurantsSearchResults> GetRestaurantsByPostCode(string postcode)
        {
            return await Task.Run(() => GetMockedRestaurantsSearchResults());
        }

        private RestaurantsSearchResults GetMockedRestaurantsSearchResults(string postcode = "PO5 7CD")
        {
            return new RestaurantsSearchResults
            {
                MetaData = new Metadata
                {
                    Latitude = 59.123f,
                    Longitude = 0.123f,
                    Postcode = postcode,
                    ResultCount = 12
                },
                Restaurants = new List<Restaurant>
                 {
                     new Restaurant{ Name = "R1" },
                     new Restaurant{ Name = "R2" },
                     new Restaurant{ Name = "R3" },
                     new Restaurant{ Name = "R4" },
                     new Restaurant{ Name = "R5" },
                     new Restaurant{ Name = "R6" },
                     new Restaurant{ Name = "R7" },
                     new Restaurant{ Name = "R8" },
                     new Restaurant{ Name = "R9" },
                     new Restaurant{ Name = "R10" },
                     new Restaurant{ Name = "R11" },
                     new Restaurant{ Name = "R12" }
                 }

            };
        }

    }
}