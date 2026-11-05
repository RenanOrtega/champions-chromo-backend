using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Application.Cupoms;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Enums;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;
using MongoDB.Bson;

namespace ChampionsChromo.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IAlbumRepository albumRepository,
    ICupomRepository cupomRepository) : IRequestHandler<CreateOrderCommand, Result<CreateOrderSummaryDto>>
{
    // Same defaults the store uses when an album has no price set for a sticker type.
    private const decimal DefaultCommonPrice = 1m;
    private const decimal DefaultLegendPrice = 5m;
    private const decimal DefaultA4Price = 15m;

    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IAlbumRepository _albumRepository = albumRepository;
    private readonly ICupomRepository _cupomRepository = cupomRepository;

    public async Task<Result<CreateOrderSummaryDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var albumOrders = new List<AlbumOrder>();
        var subtotal = 0m;

        foreach (var item in request.Albums)
        {
            if (!ObjectId.TryParse(item.AlbumId, out _))
                return Result<CreateOrderSummaryDto>.Failure("Álbum inválido.");

            var album = await _albumRepository.GetByIdAsync(item.AlbumId);
            if (album is null)
                return Result<CreateOrderSummaryDto>.Failure("Álbum não encontrado.");

            var stickers = item.Stickers.Select(sticker => new StickerOrderItem
            {
                Type = sticker.Type,
                Number = sticker.Number,
                Quantity = sticker.Quantity
            }).ToList();

            subtotal += stickers.Sum(s => PriceFor(album, s.Type) * s.Quantity);

            albumOrders.Add(new AlbumOrder
            {
                AlbumId = album.Id,
                SchoolId = album.SchoolId,
                AlbumName = album.Name,
                Stickers = stickers
            });
        }

        subtotal = Math.Round(subtotal, 2);

        Cupom? coupon = null;
        var discount = 0m;

        if (!string.IsNullOrWhiteSpace(request.CouponCode))
        {
            coupon = await _cupomRepository.FindByCodeAsync(request.CouponCode.Trim());
            if (coupon is null)
                return Result<CreateOrderSummaryDto>.Failure("Cupom não encontrado.");

            var reason = CouponRules.GetUnavailableReason(coupon, DateTime.UtcNow);
            if (reason is not null)
                return Result<CreateOrderSummaryDto>.Failure(reason);

            if (subtotal >= coupon.MinPurchaseValue)
            {
                if (!await _cupomRepository.TryConsumeAsync(coupon.Id))
                    return Result<CreateOrderSummaryDto>.Failure("Cupom esgotado.");

                discount = CouponRules.CalculateDiscount(coupon, subtotal);
            }
            else
            {
                // Same as the store: below the minimum the coupon is simply not applied.
                coupon = null;
            }
        }

        var entity = new OrderSummary
        {
            Albums = albumOrders,
            Customer = new Customer
            {
                Name = request.Customer?.Name.Trim() ?? string.Empty,
                Email = request.Customer?.Email.Trim() ?? string.Empty,
                Address = request.Customer?.Address is not null ? new CustomerAddress
                {
                    Street = request.Customer.Address.Street,
                    Number = request.Customer.Address.Number,
                    Neighborhood = request.Customer.Address.Neighborhood,
                    PostalCode = request.Customer.Address.PostalCode,
                    City = request.Customer.Address.City,
                    Complement = request.Customer.Address.Complement,
                    State = request.Customer.Address.State
                } : null
            },
            Subtotal = subtotal,
            CouponCode = coupon?.Code,
            Discount = discount,
            PriceTotal = Math.Max(0m, subtotal - discount)
        };

        try
        {
            await _orderRepository.AddAsync(entity);
            return Result<CreateOrderSummaryDto>.Success(new CreateOrderSummaryDto { Id = entity.Id });
        }
        catch (Exception ex)
        {
            if (coupon is not null)
                await _cupomRepository.ReleaseAsync(coupon.Id);

            return Result<CreateOrderSummaryDto>.Failure($"Failed to create order: {ex.Message}");
        }
    }

    private static decimal PriceFor(Album album, StickerType type) => type switch
    {
        StickerType.Common => album.CommonPrice > 0 ? album.CommonPrice : DefaultCommonPrice,
        StickerType.Legend => album.LegendPrice > 0 ? album.LegendPrice : DefaultLegendPrice,
        StickerType.a4 => album.A4Price > 0 ? album.A4Price : DefaultA4Price,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };
}
