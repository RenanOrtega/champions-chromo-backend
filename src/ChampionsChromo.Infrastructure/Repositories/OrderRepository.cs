using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class OrderRepository(MongoDbContext context) : Repository<OrderSummary>(context), IOrderRepository
{
    // Fuso usado para agrupar as vendas por dia, para que o dia do gráfico bata com o dia local da loja
    private const string DashboardTimezone = "America/Sao_Paulo";

    public async Task<MetricsDto> GetDashboardMetricsAsync(DateTime? from = null, DateTime? to = null)
    {
        // Filtrar por data (sem limites = período todo)
        var createdAtFilter = new BsonDocument();
        if (from.HasValue)
            createdAtFilter.Add("$gte", DateTime.SpecifyKind(from.Value, DateTimeKind.Utc));
        if (to.HasValue)
            createdAtFilter.Add("$lt", DateTime.SpecifyKind(to.Value, DateTimeKind.Utc));

        var matchStage = createdAtFilter.ElementCount > 0
            ? new BsonDocument("$match", new BsonDocument("CreatedAt", createdAtFilter))
            : new BsonDocument("$match", new BsonDocument());

        var pipeline = new[]
        {
            matchStage,
            
            // Adicionar campos calculados
            new BsonDocument("$addFields", new BsonDocument
            {
                { "TotalStickers", new BsonDocument("$sum", new BsonDocument("$map", new BsonDocument
                {
                    { "input", "$Albums" },
                    { "as", "album" },
                    { "in", new BsonDocument("$sum", new BsonDocument("$map", new BsonDocument
                    {
                        { "input", "$$album.Stickers" },
                        { "as", "sticker" },
                        { "in", "$$sticker.Quantity" }
                    })) }
                })) },
                { "DateOnly", new BsonDocument("$dateToString", new BsonDocument
                {
                    { "format", "%Y-%m-%d" },
                    { "date", "$CreatedAt" },
                    { "timezone", DashboardTimezone }
                }) }
            }),

            // Facet para múltiplas agregações
            new BsonDocument("$facet", new BsonDocument
            {
                // Métricas gerais
                { "generalMetrics", new BsonArray
                {
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", BsonNull.Value },
                        { "totalOrders", new BsonDocument("$sum", 1) },
                        { "totalRevenue", new BsonDocument("$sum", "$PriceTotal") },
                        { "totalStickers", new BsonDocument("$sum", "$TotalStickers") }
                    })
                }},
                
                // Vendas diárias
                { "dailySales", new BsonArray
                {
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", "$DateOnly" },
                        { "ordersCount", new BsonDocument("$sum", 1) },
                        { "revenue", new BsonDocument("$sum", "$PriceTotal") }
                    }),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                }},
                
                // Distribuição de tipos de stickers
                { "stickerTypes", new BsonArray
                {
                    new BsonDocument("$unwind", "$Albums"),
                    new BsonDocument("$unwind", "$Albums.Stickers"),
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", "$Albums.Stickers.Type" },
                        { "quantity", new BsonDocument("$sum", "$Albums.Stickers.Quantity") }
                    }),
                    new BsonDocument("$sort", new BsonDocument("quantity", -1))
                }}
            })
        };

        var aggregationResult = await _collection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

        if (aggregationResult == null)
        {
            return new MetricsDto();
        }

        var generalMetrics = aggregationResult["generalMetrics"].AsBsonArray.FirstOrDefault()?.AsBsonDocument;
        var dailySales = aggregationResult["dailySales"].AsBsonArray;
        var stickerTypes = aggregationResult["stickerTypes"].AsBsonArray;

        // Calcular total de tipos de stickers para percentuais - usando ToInt64() para conversão segura
        var totalStickerQuantity = stickerTypes.Sum(s => s.AsBsonDocument["quantity"].ToInt64());

        var metrics = new MetricsDto
        {
            OrderMetrics = new OrderMetricsDto
            {
                TotalOrders = generalMetrics?["totalOrders"]?.ToInt64() ?? 0,
                TotalRevenue = generalMetrics?["totalRevenue"]?.ToDecimal() ?? 0,
                TotalStickersOrdered = generalMetrics?["totalStickers"]?.ToInt64() ?? 0,
                AverageOrderValue = generalMetrics != null && generalMetrics["totalOrders"].ToInt64() > 0
                    ? generalMetrics["totalRevenue"].ToDecimal() / generalMetrics["totalOrders"].ToInt64()
                    : 0
            },

            DailySales = [.. dailySales.Select(d => new DailySalesDto
            {
                Date = DateTime.Parse(d.AsBsonDocument["_id"].AsString),
                OrdersCount = d.AsBsonDocument["ordersCount"].ToInt64(),
                Revenue = d.AsBsonDocument["revenue"].ToDecimal()
            })],

            StickerTypeDistribution = [.. stickerTypes.Select(s => new StickerTypeDistributionDto
            {
                Type = s.AsBsonDocument["_id"].AsInt32,
                TypeName = GetStickerTypeName(s.AsBsonDocument["_id"].AsInt32),
                Quantity = s.AsBsonDocument["quantity"].ToInt64(),
                Percentage = totalStickerQuantity > 0
                    ? Math.Round((decimal)s.AsBsonDocument["quantity"].ToInt64() / totalStickerQuantity * 100, 2)
                    : 0
            })]
        };

        return metrics;
    }

    private string GetStickerTypeName(int type)
    {
        return type switch
        {
            0 => "Comum",
            1 => "Legend",
            2 => "A4",
            _ => "Desconhecido"
        };
    }

    public async Task<IEnumerable<OrderSummary>> GetAllOrderedByCreatedAsync(bool descending = true)
    {
        var find = _collection.Find(_ => true);

        var sorted = descending
            ? find.SortByDescending(x => x.CreatedAt)
            : find.SortBy(x => x.CreatedAt);

        return await sorted.ToListAsync();
    }

    public async Task SetPaymentAsync(string id, OrderPayment payment, OrderStatusChange? statusChange = null)
    {
        var filter = Builders<OrderSummary>.Filter.Eq(a => a.Id, id);
        var update = Builders<OrderSummary>.Update
            .Set(a => a.Payment, payment)
            .Set(a => a.UpdatedAt, DateTime.UtcNow);

        if (statusChange is not null)
        {
            update = update
                .Set(a => a.Status, statusChange.Status)
                .Push(a => a.StatusHistory, statusChange);
        }

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task SetStatusAsync(string id, OrderStatusChange statusChange)
    {
        var filter = Builders<OrderSummary>.Filter.Eq(a => a.Id, id);
        var update = Builders<OrderSummary>.Update
            .Set(a => a.Status, statusChange.Status)
            .Push(a => a.StatusHistory, statusChange)
            .Set(a => a.UpdatedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<long> BackfillLegacyStatusAsync(OrderStatusChange statusChange)
    {
        // Only documents that never had the field, so running it again (or on several machines) changes nothing.
        var filter = Builders<OrderSummary>.Filter.Exists(a => a.Status, false);
        var update = Builders<OrderSummary>.Update
            .Set(a => a.Status, statusChange.Status)
            .Set(a => a.StatusHistory, new List<OrderStatusChange> { statusChange });

        var result = await _collection.UpdateManyAsync(filter, update);
        return result.ModifiedCount;
    }

    public async Task UpdateAsync(string Id, UpdateOrderDto updateOrderDto)
    {
        var filter = Builders<OrderSummary>.Filter.Eq(a => a.Id, Id);

        var update = Builders<OrderSummary>.Update
            .Set(a => a.UpdatedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update);
    }
}
