namespace ChampionsChromo.Core.Models
{
    public class WebhookEvent
    {
        public string Type { get; set; } = string.Empty;
        public object Data { get; set; } = new();
    }
}
