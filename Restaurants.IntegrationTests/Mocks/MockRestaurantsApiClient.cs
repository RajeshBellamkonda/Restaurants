using Restaurants.JustEat.Client;
using Restaurants.JustEat.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.IntegrationTests.Mocks
{
    public class MockRestaurantsApiClient : IRestaurantsApiClient
    {
        public async Task<RestaurantsClientResponse> GetRestaurantsByLatLong(string latitude, string longitude)
        {
            return await Task.Run(() => GetMockedRestaurantsSearchResults());
        }

        public async Task<RestaurantsClientResponse> GetRestaurantsByPostCode(string postcode)
        {
            return await Task.Run(() => GetMockedRestaurantsSearchResults());
        }

        private RestaurantsClientResponse GetMockedRestaurantsSearchResults(string postcode = "PO5 7CD")
        {
            return new RestaurantsClientResponse
            {
                IsSuccess = true,
                RestaurantsSearchResults = new RestaurantsSearchResults
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
                     new Restaurant{ Name = "R1", LogoUrl= "http://logo1", RatingStars=3, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R2" , LogoUrl= "http://logo2", RatingStars=4, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R3" , LogoUrl= "http://logo3", RatingStars=4, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R4" , LogoUrl= "http://logo4", RatingStars=5, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R5" , LogoUrl= "http://logo5", RatingStars=2, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R6" , LogoUrl= "http://logo6", RatingStars=5, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R7" , LogoUrl= "http://logo7", RatingStars=7, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R8" , LogoUrl= "http://logo8", RatingStars=2, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R9" , LogoUrl= "http://logo9", RatingStars=7, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R10", LogoUrl= "http://logo10", RatingStars=3, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R11", LogoUrl= "http://logo11", RatingStars=1, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                     new Restaurant{ Name = "R12", LogoUrl= "http://logo12", RatingStars=9, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new Cuisine { Name="C1" }, new Cuisine { Name = "C2" }}},
                 }

                }
            };
        }

    }
}