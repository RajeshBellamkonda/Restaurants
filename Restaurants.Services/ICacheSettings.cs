namespace Restaurants.Services
{
    public interface ICacheSettings
    {
        int ExpiryInMinutes { get; set; }
    }
}