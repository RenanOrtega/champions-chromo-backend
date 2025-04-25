using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.CreateOrder;

public class CreateOrderCommandHandler(IPixRepository pixRepository) : IRequestHandler<CreateOrderCommand, Result>
{
    private readonly IPixRepository _pixRepository = pixRepository;

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var pixOrder = new PixOrder
        {
            Customer = request.Customer,
            Address = request.Address,
            IntegrationId = request.IntegrationId,
            Payment = request.Payment,
            Status = request.Status,
        };

        try
        {
            await _pixRepository.AddAsync(pixOrder);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create pix order: {ex.Message}");
        }
    }
}
