using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler(IPixRepository pixRepository) : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IPixRepository _pixRepository = pixRepository;

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var pixOrder = await _pixRepository.FindByIntegrationId(request.IntegrationId);
        if (pixOrder is null)
        {
            return Result.Failure($"Pix order with integration ID {request.IntegrationId} not found.");
        }

        pixOrder.Status = request.Status;
        await _pixRepository.UpdateAsync(pixOrder.Id, pixOrder);
        return Result.Success();
    }
}
