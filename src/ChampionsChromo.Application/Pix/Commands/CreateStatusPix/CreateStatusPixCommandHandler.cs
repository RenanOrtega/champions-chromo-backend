using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.CreateStatusPix;

public class CreateStatusPixCommandHandler(IPixRepository pixRepository) : IRequestHandler<CreateStatusPixCommand, Result>
{
    private readonly IPixRepository _pixRepository = pixRepository;
    public async Task<Result> Handle(CreateStatusPixCommand request, CancellationToken cancellationToken)
    {
        var entity = new PixStatus
        {
            Amount = request.Payment.Amount,
            CustomerId = request.PixQrCode.Customer?.Id ?? "",
            Fee = request.Payment.Fee,
            PixId = request.PixQrCode.Id,
            Status = request.PixQrCode.Status,
        };
        try
        {
            await _pixRepository.AddAsync(entity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create status pix: {ex.Message}");
        }
    }
}
