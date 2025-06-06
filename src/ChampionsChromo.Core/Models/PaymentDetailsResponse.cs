namespace ChampionsChromo.Core.Models
{
    public class PaymentDetailsResponse
    {
        public string Id { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}
