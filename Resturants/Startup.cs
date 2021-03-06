using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurants.JustEat.Client;
using Restaurants.Mappers;
using Restaurants.Services;

namespace Restaurants
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            var cacheSettings = new CacheSettings();
            Configuration.GetSection(CacheSettings.CacheSettingsName).Bind(cacheSettings);
            services.AddSingleton<ICacheSettings>(cacheSettings);
            var justEatApiClientSettings = new JustEatApiClientSettings();
            Configuration.GetSection(JustEatApiClientSettings.JustEatApiClientSettingName).Bind(justEatApiClientSettings);
            services.AddSingleton<IJustEatApiClientSettings>(justEatApiClientSettings);

            services.AddControllersWithViews();
            services.AddHttpClient<IRestaurantsApiClient, RestaurantsApiClient>();
            services.AddAutoMapper(typeof(RestaurantsMapper));
            services.AddTransient<IRestaurantsService, RestaurantsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/healthcheck");
            });
        }
    }
}
