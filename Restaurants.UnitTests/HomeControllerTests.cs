using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Controllers;
using Restaurants.Mappers;
using Restaurants.Services;
using Restaurants.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
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
        public async Task IndexReturnsEmptyModelAsync()
        {
            // Arrange
            //_homeController.HttpContext= new HttpContext() { Request = new HttpRequest { } }

            // Act
            var result = await _homeController.Index(new RestaurantsSearchViewModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RestaurantsSearchViewModel>(viewResult.ViewData.Model);
            Assert.False(model.DisplayError);
        }



    }
}
