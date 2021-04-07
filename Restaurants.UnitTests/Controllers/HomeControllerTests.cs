using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PagedList.Core;
using Restaurants.Controllers;
using Restaurants.Mappers;
using Restaurants.Services;
using Restaurants.Services.DTOs;
using Restaurants.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.UnitTests
{
    public class HomeControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRestaurantsService> _restaurantsServiceMock;
        private readonly HomeController _homeController;

        public HomeControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(new Profile[] { new RestaurantsMapper() });

            });
            _mapper = mappingConfig.CreateMapper();
            _restaurantsServiceMock = new Mock<IRestaurantsService>();

            _homeController = new HomeController(
                new Mock<ILogger<HomeController>>().Object,
                _mapper,
                _restaurantsServiceMock.Object);
        }

        [Fact]
        public async Task GetIndexReturnsEmptyModel()
        {
            // Act
            var result = await _homeController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.False(model.DisplayError);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async void GetIndexReturnsResultsForPostCodeQueryParameter()
        {
            // Arrange
            var postcode = "PO5 7CD";
            var page = 1;
            var pageSize = 10;
            var mockedSearchResults = GetMockedRestaurantsSearchResults();

            _restaurantsServiceMock.Setup(x => x.GetRestaurantsByPostCode(postcode, page, pageSize))
                .ReturnsAsync(mockedSearchResults);

            // Act
            var result = await _homeController.Index(postcode);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.False(model.DisplayError);
            Assert.Equal(postcode, model.PostCode);
            Assert.NotEmpty(model.Restaurants.ToList());
            Assert.Equal(pageSize, model.Restaurants.Count);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
        }

        [Fact]
        public async void IndexReturnsResultsForPostCodeQueryParameterWithPage2()
        {
            // Arrange
            var postcode = "PO5 7CD";
            var page = 2;
            var pageSize = 10;
            var mockedSearchResults = GetMockedRestaurantsSearchResults(page: page);

            _restaurantsServiceMock.Setup(x => x.GetRestaurantsByPostCode(postcode, page, pageSize))
                .ReturnsAsync(mockedSearchResults);

            // Act
            var result = await _homeController.Index(postcode, page: page);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.False(model.DisplayError);
            Assert.Equal(postcode, model.PostCode);
            Assert.NotEmpty(model.Restaurants.ToList());
            Assert.Equal(2, model.Restaurants.Count);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
        }

        [Fact]
        public async void GetIndexReturnsNoResultsForInvalidPostCodeQueryParameter()
        {
            // Arrange
            var postcode = "ABCD";
            var page = 1;
            var pageSize = 10;
            var expectedErrorMessage = "Error Message";
            var mockedSearchResults = GetMockedRestaurantsSearchResults(postcode);
            mockedSearchResults.Restaurants = null;
            mockedSearchResults.ErrorMessage = expectedErrorMessage;

            _restaurantsServiceMock.Setup(x => x.GetRestaurantsByPostCode(postcode, page, pageSize))
                .ReturnsAsync(mockedSearchResults);

            // Act
            var result = await _homeController.Index(postcode);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.True(model.DisplayError);
            Assert.Equal(postcode, model.PostCode);
            Assert.Null(model.Restaurants);
            Assert.Equal(expectedErrorMessage, model.ErrorMessage);

            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
        }

        [Fact]
        public async void GetIndexReturnsResultsForGeoLocationQueryParameters()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var latitude = "59.123";
            var longitude = "0.123";
            var mockedSearchResults = GetMockedRestaurantsSearchResults();

            _restaurantsServiceMock.Setup(x => x.GetRestaurantsByGeoLocation(latitude, longitude, page, pageSize))
                .ReturnsAsync(mockedSearchResults);

            // Act
            var result = await _homeController.Index(latitude: latitude, longitude: longitude);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.False(model.DisplayError);
            Assert.NotNull(model.PostCode);
            Assert.NotEmpty(model.Restaurants.ToList());
            Assert.Equal(pageSize, model.Restaurants.Count);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async void GetIndexReturnsNoResultsForInvalidGeoLocationQueryParameters()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var latitude = "53";
            var longitude = "0123";
            var expectedErrorMessage = "Error Message";
            var mockedSearchResults = GetMockedRestaurantsSearchResults();
            mockedSearchResults.Restaurants = null;
            mockedSearchResults.PostCode = null;
            mockedSearchResults.ErrorMessage = expectedErrorMessage;

            _restaurantsServiceMock.Setup(x => x.GetRestaurantsByGeoLocation(latitude, longitude, page, pageSize))
                .ReturnsAsync(mockedSearchResults);

            // Act
            var result = await _homeController.Index(latitude: latitude, longitude: longitude);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.True(model.DisplayError);
            Assert.Null(model.PostCode);
            Assert.Null(model.Restaurants);
            Assert.Equal(expectedErrorMessage, model.ErrorMessage);

            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async void GetIndexReturnsResultsForGeoLocationQueryParametersWithPage2()
        {
            // Arrange
            var page = 2;
            var pageSize = 10;
            var latitude = "59.123";
            var longitude = "0.123";
            var mockedSearchResults = GetMockedRestaurantsSearchResults(page: page);

            _restaurantsServiceMock.Setup(x => x.GetRestaurantsByGeoLocation(latitude, longitude, page, pageSize))
                .ReturnsAsync(mockedSearchResults);

            // Act
            var result = await _homeController.Index(latitude: latitude, longitude: longitude, page: page);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.False(model.DisplayError);
            Assert.Equal(latitude, model.Latitude);
            Assert.Equal(longitude, model.Longitude);
            Assert.NotNull(model.PostCode);
            Assert.NotEmpty(model.Restaurants.ToList());
            Assert.Equal(2, model.Restaurants.Count);

            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }


        [Fact]
        public async Task PostInvalidIndexReturnsErrorModelAsync()
        {
            // Arrange
            _homeController.ModelState.AddModelError("PostCode", "PostCode is required");

            // Act
            var result = await _homeController.Index(new RestaurantsSearchViewModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.False(model.DisplayError);

            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task PostValidIndexReturnsResultsForPostCodeSearch()
        {
            var postcode = "PO5 7CD";
            var page = 1;
            var pageSize = 10;
            var mockedSearchResults = GetMockedRestaurantsSearchResults();

            _restaurantsServiceMock.Setup(x => x.GetRestaurantsByPostCode(postcode, page, pageSize))
                .ReturnsAsync(mockedSearchResults);

            // Act
            var result = await _homeController.Index(new RestaurantsSearchViewModel { PostCode = postcode });

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.False(model.DisplayError);
            Assert.Equal(postcode, model.PostCode);
            Assert.NotEmpty(model.Restaurants.ToList());
            Assert.Equal(pageSize, model.Restaurants.Count);

            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByGeoLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _restaurantsServiceMock.Verify(x => x.GetRestaurantsByPostCode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
        }


        private RestaurantSearchResultsDto GetMockedRestaurantsSearchResults(string postcode = "PO5 7CD", int page = 1, int pageSize = 10)
        {
            return new RestaurantSearchResultsDto
            {
                Latitude = "59.123",
                Longitude = "0.123",
                PostCode = postcode,
                Restaurants = new StaticPagedList<RestaurantDto>(
                    MockedRestaurants.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                    page, pageSize, MockedRestaurants.Count)
            };

        }
        private List<RestaurantDto> MockedRestaurants => new List<RestaurantDto>
        {
            new RestaurantDto { Name = "R1", LogoUrl= "http://logo1", RatingStars=3, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R2" , LogoUrl= "http://logo2", RatingStars=4, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R3" , LogoUrl= "http://logo3", RatingStars=4, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R4" , LogoUrl= "http://logo4", RatingStars=5, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R5" , LogoUrl= "http://logo5", RatingStars=2, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R6" , LogoUrl= "http://logo6", RatingStars=5, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R7" , LogoUrl= "http://logo7", RatingStars=7, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R8" , LogoUrl= "http://logo8", RatingStars=2, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R9" , LogoUrl= "http://logo9", RatingStars=7, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R10", LogoUrl= "http://logo10", RatingStars=3, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R11", LogoUrl= "http://logo11", RatingStars=1, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
            new RestaurantDto { Name = "R12", LogoUrl= "http://logo12", RatingStars=9, NumberOfRatings=50, RatingAverage=3, Cuisines= new []{ new CuisineDto { Name="C1" }, new CuisineDto { Name = "C2" }}},
        };

    }
}
