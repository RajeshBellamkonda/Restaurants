using AutoMapper;
using PagedList.Core;
using Restaurants.Services.DTOs;
using Restaurants.ViewModels;
using System.Collections.Generic;

namespace Restaurants.Mappers
{
    public class PagedRestaurantViewModel : IValueResolver<RestaurantSearchResultsDto, RestaurantsSearchViewModel, StaticPagedList<RestaurantViewModel>>
    {
        public StaticPagedList<RestaurantViewModel> Resolve(RestaurantSearchResultsDto source, RestaurantsSearchViewModel destination, StaticPagedList<RestaurantViewModel> destMember, ResolutionContext context)
        {
            if (source.Restaurants == null) return default;
            var mappedResults = context.Mapper.Map<IEnumerable<RestaurantViewModel>>(source.Restaurants);
            return new StaticPagedList<RestaurantViewModel>(mappedResults, source.Restaurants.GetMetaData());

        }
    }
}