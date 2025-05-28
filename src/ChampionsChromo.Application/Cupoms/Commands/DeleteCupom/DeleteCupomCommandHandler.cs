using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Commands.DeleteCupom;

public class DeleteCupomCommandHandler(ICupomRepository cupomRepository) : IRequestHandler<DeleteCupomCommand, Result>
{
    private readonly ICupomRepository _cupomRepository = cupomRepository;

    public async Task<Result> Handle(DeleteCupomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _cupomRepository.DeleteAsync(request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failure to delete cupom: {ex.Message}");
        }
    }
}
