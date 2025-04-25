using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Pix.Queries.GetPixOrderStatus;

public class GetPixOrderStatusQueryHandler(IPixRepository pixRepository) : IRequestHandler<GetPixOrderStatusQuery, Result<PixOrderStatus>>
{
    private readonly IPixRepository _pixRepository = pixRepository;

    public async Task<Result<PixOrderStatus>> Handle(GetPixOrderStatusQuery request, CancellationToken cancellationToken)
    {
        var pixOrder = await _pixRepository.FindByIntegrationId(request.IntegrationId);
        if (pixOrder is null)
        {
            return Result<PixOrderStatus>.Failure($"Pix order with integration ID {request.IntegrationId} not found.");
        }

        return Result<PixOrderStatus>.Success(new PixOrderStatus { Status = pixOrder.Status });
    }
}
