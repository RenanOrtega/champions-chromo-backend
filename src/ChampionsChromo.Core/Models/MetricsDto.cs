namespace ChampionsChromo.Core.Models;

public class MetricsDto
{
    public long AlbumsCount { get; set; }
    public long SchoolsCount { get; set; }
    public OrderMetricsDto OrderMetrics { get; set; } = new();
    public List<DailySalesDto> DailySales { get; set; } = [];
    public List<StickerTypeDistributionDto> StickerTypeDistribution { get; set; } = [];
}

public class OrderMetricsDto
{
    public long TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public long TotalStickersOrdered { get; set; }
}

public class DailySalesDto
{
    public DateTime Date { get; set; }
    public long OrdersCount { get; set; }
    public decimal Revenue { get; set; }
}

public class StickerTypeDistributionDto
{
    public int Type { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public long Quantity { get; set; }
    public decimal Percentage { get; set; }
}