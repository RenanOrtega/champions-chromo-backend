using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, Result<CreateOrderSummaryDto>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<CreateOrderSummaryDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = new OrderSummary
        {
            Albums = [.. request.Albums.Select(album => new AlbumOrder
            {
                AlbumId = album.AlbumId,
                SchoolId = album.SchoolId,
                Stickers = [.. album.Stickers.Select(sticker => new StickerOrderItem
                {
                    Type = sticker.Type,
                    Number = sticker.Number,
                    Quantity = sticker.Quantity
                })]
            })],
            PriceTotal = request.PriceTotal
        };

        try
        {
            await _orderRepository.AddAsync(entity);
            return Result<CreateOrderSummaryDto>.Success(new CreateOrderSummaryDto { Id = entity.Id });
        }
        catch (Exception ex)
        {
            return Result<CreateOrderSummaryDto>.Failure($"Failed to create order: {ex.Message}");
        }
    }
}
