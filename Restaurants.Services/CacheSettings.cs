namespace Restaurants.Services
{
    public class CacheSettings : ICacheSettings
    {
        public const string CacheSettingsName = "CacheSettings";
        public int ExpiryInMinutes { get; set; } = 1;
    }
}