using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Clients.Interfaces;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Entities.Pix;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.CreateOrder;

public class CreateOrderCommandHandler(IPixRepository pixRepository, IAbacatePayClient abacatePayClient) : IRequestHandler<CreateOrderCommand, Result<GeneratePixAbacatePayResponse>>
{
    private readonly IPixRepository _pixRepository = pixRepository;
    private readonly IAbacatePayClient _abacatePayClient = abacatePayClient;

    public async Task<Result<GeneratePixAbacatePayResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var generatePixRequest = new GeneratePixAbacatePayRequest 
        {
            Amount = request.Payment.Amount,
            Customer = request.Customer, 
            ExpiresIn = request.ExpiresIn, 
            Description = request.Description 
        };
        
        var response = await _abacatePayClient.GeneratePix("/v1/pixQrCode/create", generatePixRequest, cancellationToken);
        if (response is null)
            return Result<GeneratePixAbacatePayResponse>.Failure("Failed to generate pix QR code.");
        
        if (response.Data is null)
            return Result<GeneratePixAbacatePayResponse>.Failure("Failed to generate pix QR code: No data returned.");

        var pixOrder = new PixOrder
        {
            Customer = request.Customer,
            Address = request.Address,
            IntegrationId = response.Data.Id,
            Payment = new Payment { Amount = generatePixRequest.Amount, Fee = response.Data.PlatformFee},
            Status = response.Data.Status,
        };

        try
        {
            await _pixRepository.AddAsync(pixOrder);
            return Result<GeneratePixAbacatePayResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GeneratePixAbacatePayResponse>.Failure($"Failed to create pix order: {ex.Message}");
        }
    }
}
