using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.CreateStatusPix;

public record CreateStatusPixCommand : IRequest<Result>
{
    public required PaymentData Payment { get; set; }
    public required PixQrCodeData PixQrCode { get; set; }
    public bool DevMode { get; set; }
    public string Event { get; set; } = string.Empty;
}

public class PaymentData
{
    public int Amount { get; set; }
    public int Fee { get; set; }
    public string Method { get; set; } = string.Empty;
}

public class PixQrCodeData
{
    public int Amount { get; set; }
    public string Id { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public CustomerData? Customer { get; set; }
}

public class CustomerData
{
    public string Id { get; set; } = string.Empty;
    public CustomerMetadata? Metadata { get; set; }
}

public class CustomerMetadata
{
    public string Cellphone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
}
