namespace Restaurants.JustEat.Client.Models
{
    public class Deal
    {
        public string Description { get; set; }
        public float DiscountPercent { get; set; }
        public float QualifyingPrice { get; set; }
        public string OfferType { get; set; }
        public string OfferId { get; set; }
        public object CampaignId { get; set; }
        public string ConsumerSegment { get; set; }
    }

}
