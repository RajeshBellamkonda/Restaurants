using AutoMapper;
using Restaurants.JustEat.Client.Models;
using Restaurants.Services.DTOs;
using Restaurants.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Mappers
{
    public class RestaurantsMapper : Profile
    {
        public RestaurantsMapper()
        {
            // DTOs to ViewModels
            CreateMap<RestaurantDto, RestaurantViewModel>();
            CreateMap<CuisineDto, CuisineViewModel>();
            CreateMap<RestaurantSearchResultsDto, RestaurantsSearchViewModel>()
                .ForMember(x => x.Restaurants, opt => opt.AllowNull())
                .ForMember(x => x.Restaurants, opt => opt.MapFrom<PagedRestaurantViewModel>());

            // Models to ViewModels
            CreateMap<Restaurant, RestaurantDto>();
            CreateMap<Cuisine, CuisineDto>();
        }
    }
}
