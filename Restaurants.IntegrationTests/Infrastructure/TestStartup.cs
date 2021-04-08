using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.IntegrationTests.Mocks;
using Restaurants.JustEat.Client;

namespace Restaurants.IntegrationTests.Infrastructure
{
    internal class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton<IRestaurantsApiClient, MockRestaurantsApiClient>();
        }

    }
}