namespace Restaurants.JustEat.Client.Models
{
    public class Offer
    {
        public string Type { get; set; }
        public float Amount { get; set; }
        public float QualifyingValue { get; set; }
        public float MaxQualifyingValue { get; set; }
        public string Description { get; set; }
        public string ConsumerSegment { get; set; }
        public string OfferId { get; set; }
    }

}
