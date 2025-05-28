using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Commands.UpdateCupom;

public class UpdateCupomCommandHandler(ICupomRepository cupomRepository) : IRequestHandler<UpdateCupomCommand, Result>
{
    private readonly ICupomRepository _cupomRepository = cupomRepository;

    public async Task<Result> Handle(UpdateCupomCommand request, CancellationToken cancellationToken)
    {
        var cupomUpdateDto = new UpdateCupomDto
        {
            IsActive = request.IsActive,
            MinPurchaseValue = request.MinPurchaseValue,
            ExpiresAt = request.ExpiresAt,
            UsageLimit = request.UsageLimit,
            Value = request.Value,
            Type = request.Type
        };

        try
        {
            await _cupomRepository.UpdateAsync(request.Id, cupomUpdateDto);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create album: {ex.Message}");
        }
    }
}
