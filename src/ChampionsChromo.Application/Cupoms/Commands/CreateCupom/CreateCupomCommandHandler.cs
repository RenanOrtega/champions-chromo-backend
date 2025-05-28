using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Commands.CreateCupom;

public class CreateCupomCommandHandler(ICupomRepository cupomRepository) : IRequestHandler<CreateCupomCommand, Result>
{
    private readonly ICupomRepository _cupomRepository = cupomRepository;

    public async Task<Result> Handle(CreateCupomCommand request, CancellationToken cancellationToken)
    {
        var entity = new Cupom
        {
            Code = request.Code,
            Type = request.Type,
            Value = request.Value,
            UsageLimit = request.UsageLimit,
            UsedCount = request.UsedCount,
            ExpiresAt = request.ExpiresAt,
            MinPurchaseValue = request.MinPurchaseValue,
            IsActive = request.IsActive
        };

        try
        {
            await _cupomRepository.AddAsync(entity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create cupom: {ex.Message}");
        }
    }
}
