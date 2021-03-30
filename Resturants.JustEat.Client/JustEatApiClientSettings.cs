namespace Restaurants.JustEat.Client
{
    public class JustEatApiClientSettings : IJustEatApiClientSettings
    {
        public const string JustEatApiClientSettingName = "JustEatApiClientSettings";
        public string BaseAddress { get; set; }
    }
}