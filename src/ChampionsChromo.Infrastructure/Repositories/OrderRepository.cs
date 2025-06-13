using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Infrastructure.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChampionsChromo.Infrastructure.Repositories;

public class OrderRepository(MongoDbContext context) : Repository<OrderSummary>(context), IOrderRepository
{
    public async Task<MetricsDto> GetDashboardMetricsAsync(int daysBack = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-daysBack);

        var pipeline = new[]
        {
            // Filtrar por data
            new BsonDocument("$match", new BsonDocument
            {
                { "CreatedAt", new BsonDocument("$gte", startDate) }
            }),
            
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
                    { "date", "$CreatedAt" }
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

    public async Task UpdateAsync(string Id, UpdateOrderDto updateOrderDto)
    {
        var filter = Builders<OrderSummary>.Filter.Eq(a => a.Id, Id);

        var update = Builders<OrderSummary>.Update
            .Set(a => a.UpdatedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update);
    }
}
