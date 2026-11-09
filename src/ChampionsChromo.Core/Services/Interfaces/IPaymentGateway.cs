namespace ChampionsChromo.Core.Services.Interfaces;

public interface IPaymentGateway
{
    Task<GatewayOrder> CreatePixOrderAsync(CreatePixOrderRequest request, CancellationToken cancellationToken = default);
    Task<GatewayOrder?> GetOrderAsync(string providerOrderId, CancellationToken cancellationToken = default);
}

public record CreatePixOrderRequest(
    string ExternalReference,
    decimal Amount,
    string PayerEmail,
    string PayerFirstName,
    string IdempotencyKey,
    TimeSpan ExpiresIn);

public record GatewayOrder(
    string Id,
    string Status,
    string? StatusDetail,
    string? ExternalReference,
    decimal TotalAmount,
    string? PaymentId,
    string? QrCode,
    string? QrCodeBase64,
    string? TicketUrl);
