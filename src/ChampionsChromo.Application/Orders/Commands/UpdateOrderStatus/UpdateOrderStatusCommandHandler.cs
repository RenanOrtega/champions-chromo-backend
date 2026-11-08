using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;
using MongoDB.Bson;

namespace ChampionsChromo.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler(IOrderRepository orderRepository) : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(request.Id, out _))
            return Result.Failure("Pedido não encontrado.");

        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order is null)
            return Result.Failure("Pedido não encontrado.");

        if (order.Status == request.Status)
            return Result.Success();

        await _orderRepository.SetStatusAsync(order.Id, new OrderStatusChange
        {
            Status = request.Status,
            At = DateTime.UtcNow,
            By = string.IsNullOrWhiteSpace(request.ChangedBy) ? "admin" : request.ChangedBy,
            Note = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim()
        });

        return Result.Success();
    }
}
