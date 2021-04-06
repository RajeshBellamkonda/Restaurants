using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Restaurants.JustEat.Client;
using Restaurants.JustEat.Client.Models;
using Restaurants.Mappers;
using Restaurants.Services;
using Restaurants.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.UnitTests
{
    public class RestaurantServicePostCodeSearchTests
    {
        private readonly Mock<IRestaurantsApiClient> _restaurantsApiClientMock;
        private IRestaurantsService _restaurantsService;
        private IMapper _mapper;
        private IMemoryCache _cacheMock;

        public RestaurantServicePostCodeSearchTests()
        {
            _restaurantsApiClientMock = new Mock<IRestaurantsApiClient>();
            var loggerMock = new Mock<ILogger<RestaurantsService>>();
            IOptions<MemoryCacheOptions> options = new MemoryCacheOptions();
            _cacheMock = new MemoryCache(options);
            var cacheSettings = new CacheSettings { ExpiryInMinutes = 1 };
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(new Profile[] { new RestaurantsMapper() });

            });
            _mapper = mappingConfig.CreateMapper();

            _restaurantsService = new RestaurantsService(loggerMock.Object,
                _restaurantsApiClientMock.Object,
                _cacheMock,
                cacheSettings,
                _mapper
                );

        }

        [Fact]
        public async Task ReturnsRestaurantsForPostCode()
        {
            // Arrange
            var postcode = "po5 7cd";
            var page = 1;
            var pageSize = 10;
            var mockedSearchResults = GetMockedRestaurantsSearchResults(postcode);

            _restaurantsApiClientMock.Setup(x => x.GetRestaurantsByPostCode(postcode))
                .ReturnsAsync(mockedSearchResults);

            var expectedResult = _mapper.Map<List<RestaurantDto>>(mockedSearchResults.Restaurants).Take(pageSize).ToList();

            // Act
            var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(postcode, page, pageSize);
            var restaurantsActual = restaurantSearchResultsDto.Restaurants.ToList();

            // Assert
            Assert.NotNull(restaurantSearchResultsDto);
            Assert.Equal(expectedResult.Count, restaurantsActual.Count);
            Assert.True(restaurantsActual.TrueForAll(r => expectedResult.Select(x => x.Name).Contains(r.Name)));
            Assert.NotNull(restaurantSearchResultsDto.PostCode);
            Assert.NotNull(restaurantSearchResultsDto.Latitude);
            Assert.NotNull(restaurantSearchResultsDto.Longitude);
        }

        [Fact]
        public async Task PostCodeIsCleanedBeforeProcessing()
        {
            // Arrange
            var postcode = "PO5 7CD ";
            var page = 1;
            var pageSize = 10;
            var cleanPostCode = CleanString(postcode);

            var mockedSearchResults = GetMockedRestaurantsSearchResults(cleanPostCode);
            _restaurantsApiClientMock.Setup(x => x.GetRestaurantsByPostCode(cleanPostCode))
              .ReturnsAsync(mockedSearchResults);

            // Act
            await _restaurantsService.GetRestaurantsByPostCode(postcode, page, pageSize);

            // Assert
            _restaurantsApiClientMock.Verify(x => x.GetRestaurantsByPostCode(It.Is<string>(p => p == cleanPostCode)), Times.Exactly(1));
            _cacheMock.TryGetValue(cleanPostCode, out var actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact]
        public async Task WhenValueFoundInCacheItDoesnotCallAPI()
        {
            // Arrange
            var postcode = "po5 7cd";
            var page = 1;
            var pageSize = 10;

            var mockedSearchResults = GetMockedRestaurantsSearchResults(postcode);

            _cacheMock.Set(postcode, mockedSearchResults);

            // Act
            var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(postcode, page, pageSize);

            // Assert
            _restaurantsApiClientMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>()), Times.Never);
            Assert.NotNull(restaurantSearchResultsDto);
        }

        [Fact]
        public async Task WhenValueNotFoundInCacheItCallsAPI()
        {
            // Arrange
            var postcode = "po5 7cd";
            var page = 1;
            var pageSize = 10;

            var mockedSearchResults = GetMockedRestaurantsSearchResults(postcode);
            _restaurantsApiClientMock.Setup(x => x.GetRestaurantsByPostCode(postcode))
             .ReturnsAsync(mockedSearchResults);

            // Act
            var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(postcode, page, pageSize);

            // Assert
            _restaurantsApiClientMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>()), Times.Exactly(1));
            Assert.NotNull(restaurantSearchResultsDto);
        }

        [Fact]
        public async Task APIDataIsSavedInCacheWithProperKey()
        {
            // Arrange
            var postcode = "PO5 7CD ";
            var page = 1;
            var pageSize = 10;
            var cleanPostCode = CleanString(postcode);
            var mockedSearchResults = GetMockedRestaurantsSearchResults(cleanPostCode);
            _restaurantsApiClientMock.Setup(x => x.GetRestaurantsByPostCode(cleanPostCode))
             .ReturnsAsync(mockedSearchResults);

            // Act
            var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(postcode, page, pageSize);

            // Assert
            Assert.NotNull(restaurantSearchResultsDto);
            _cacheMock.TryGetValue(cleanPostCode, out var actualResult);
            Assert.NotNull(actualResult);

            _cacheMock.TryGetValue(postcode, out var nullResult);
            Assert.Null(nullResult);
        }

        [Fact]
        public async Task ReturnsErrorMessageWhenNoResultsFound()
        {
            // Arrange
            var postcode = "po5 7cd";
            var page = 1;
            var pageSize = 10;
            RestaurantsSearchResults mockedSearchResults = null;

            _restaurantsApiClientMock.Setup(x => x.GetRestaurantsByPostCode(postcode))
             .ReturnsAsync(mockedSearchResults);

            // Act
            var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(postcode, page, pageSize);

            // Assert
            Assert.NotNull(restaurantSearchResultsDto);
            Assert.NotNull(restaurantSearchResultsDto.ErrorMessage);
            Assert.True(!string.IsNullOrEmpty(restaurantSearchResultsDto.ErrorMessage));

            _cacheMock.TryGetValue(postcode, out var nullResult);
            Assert.Null(nullResult);
        }

        [Fact]
        public async Task ReturnsErrorMessageWhenExceptionFromAPI()
        {
            // Arrange
            var postcode = "po5 7cd";
            var page = 1;
            var pageSize = 10;

            _restaurantsApiClientMock.Setup(x => x.GetRestaurantsByPostCode(postcode))
             .ThrowsAsync(new HttpRequestException("Something went wrong"));

            // Act
            var restaurantSearchResultsDto = await _restaurantsService.GetRestaurantsByPostCode(postcode, page, pageSize);

            // Assert
            Assert.NotNull(restaurantSearchResultsDto);
            Assert.NotNull(restaurantSearchResultsDto.ErrorMessage);
            Assert.True(!string.IsNullOrEmpty(restaurantSearchResultsDto.ErrorMessage));

            _cacheMock.TryGetValue(postcode, out var nullResult);
            Assert.Null(nullResult);
        }

        [Fact]
        public async Task ReturnsCorrectPagedResultsForPostCodeAsync()
        {
            // Arrange
            var postcode = "po5 7cd";
            var page1 = 1;
            var page2 = 2;
            var pageSize = 10;
            var mockedSearchResults = GetMockedRestaurantsSearchResults(postcode);

            _restaurantsApiClientMock.Setup(x => x.GetRestaurantsByPostCode(postcode))
                .ReturnsAsync(mockedSearchResults);

            var expectedResult = _mapper.Map<List<RestaurantDto>>(mockedSearchResults.Restaurants).Take(pageSize).ToList();

            // Act
            var restaurantSearchResultsDtoP1 = await _restaurantsService.GetRestaurantsByPostCode(postcode, page1, pageSize);
            var restaurantsActualP1 = restaurantSearchResultsDtoP1.Restaurants.ToList();

            var restaurantSearchResultsDtoP2 = await _restaurantsService.GetRestaurantsByPostCode(postcode, page2, pageSize);
            var restaurantsActualP2 = restaurantSearchResultsDtoP2.Restaurants.ToList();

            // Assert
            // Page1 results
            Assert.NotNull(restaurantSearchResultsDtoP1);
            Assert.Equal(expectedResult.Count, restaurantsActualP1.Count);
            Assert.True(restaurantsActualP1.TrueForAll(r => expectedResult.Select(x => x.Name).Contains(r.Name)));

            var pagedMetaData = restaurantSearchResultsDtoP1.Restaurants.GetMetaData();
            Assert.NotNull(pagedMetaData);
            Assert.Equal(mockedSearchResults.MetaData.ResultCount, pagedMetaData.TotalItemCount);

            // page2 results
            Assert.NotNull(restaurantSearchResultsDtoP2);
            Assert.Equal(2, restaurantsActualP2.Count);
            
            var pagedMetaData2 = restaurantSearchResultsDtoP2.Restaurants.GetMetaData();
            Assert.NotNull(pagedMetaData2);
            Assert.Equal(mockedSearchResults.MetaData.ResultCount, pagedMetaData.TotalItemCount);
        }

        private static string CleanString(string postCode) => postCode.Trim().ToLower();
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
