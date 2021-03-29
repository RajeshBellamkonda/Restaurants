using AutoMapper;
using Restaurants.JustEat.Client.Models;
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
            CreateMap<Restaurant, ResturantViewModel>();
            CreateMap<Cuisine, CuisineViewModel>();
        }
    }
}
