using System;

namespace Restaurants.JustEat.Client.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueName { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float RatingStars { get; set; }
        public int NumberOfRatings { get; set; }
        public float RatingAverage { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string LogoUrl { get; set; }
        public bool IsTestRestaurant { get; set; }
        public bool IsHalal { get; set; }
        public bool IsNew { get; set; }
        public string ReasonWhyTemporarilyOffline { get; set; }
        public float DriveDistance { get; set; }
        public bool DriveInfoCalculated { get; set; }
        public bool IsCloseBy { get; set; }
        public float OfferPercent { get; set; }
        public DateTime NewnessDate { get; set; }
        public DateTime OpeningTime { get; set; }
        public object OpeningTimeUtc { get; set; }
        public DateTime OpeningTimeIso { get; set; }
        public DateTime OpeningTimeLocal { get; set; }
        public DateTime DeliveryOpeningTimeLocal { get; set; }
        public DateTime? DeliveryOpeningTime { get; set; }
        public object DeliveryOpeningTimeUtc { get; set; }
        public DateTime DeliveryStartTime { get; set; }
        public object DeliveryTime { get; set; }
        public object DeliveryTimeMinutes { get; set; }
        public int? DeliveryWorkingTimeMinutes { get; set; }
        public bool IsCollection { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsFreeDelivery { get; set; }
        public bool IsOpenNowForCollection { get; set; }
        public bool IsOpenNowForDelivery { get; set; }
        public bool IsOpenNowForPreorder { get; set; }
        public bool IsOpenNow { get; set; }
        public bool IsTemporarilyOffline { get; set; }
        public int? DeliveryMenuId { get; set; }
        public object CollectionMenuId { get; set; }
        public object DeliveryZipcode { get; set; }
        public float DeliveryCost { get; set; }
        public float MinimumDeliveryValue { get; set; }
        public float SecondDateRanking { get; set; }
        public int DefaultDisplayRank { get; set; }
        public int SponsoredPosition { get; set; }
        public float SecondDateRank { get; set; }
        public float Score { get; set; }
        public bool IsTemporaryBoost { get; set; }
        public bool IsSponsored { get; set; }
        public bool IsPremier { get; set; }
        public object HygieneRating { get; set; }
        public bool ShowSmiley { get; set; }
        public object SmileyDate { get; set; }
        public bool SmileyElite { get; set; }
        public object SmileyResult { get; set; }
        public object SmileyUrl { get; set; }
        public bool SendsOnItsWayNotifications { get; set; }
        public string BrandName { get; set; }
        public bool IsBrand { get; set; }
        public DateTime LastUpdated { get; set; }
        public string[] Tags { get; set; }
        public object[] DeliveryChargeBands { get; set; }
        public Cuisine[] Cuisines { get; set; }
        public object[] Badges { get; set; }
        public object[] OpeningTimes { get; set; }
        public object[] ServiceableAreas { get; set; }
    }

}
