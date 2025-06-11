namespace ChampionsChromo.Core.Models
{
    public class UpdateSchoolDto
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Warning { get; set; } = string.Empty;
        public string BgWarningColor { get; set; } = string.Empty;
        public int ShippingCost { get; set; }

    }
}
