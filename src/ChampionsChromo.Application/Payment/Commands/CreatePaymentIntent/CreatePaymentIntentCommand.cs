using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Payment.Commands.CreatePaymentIntent;

public record CreatePaymentIntentCommand : IRequest<Result<CreatePaymentIntentResponse>>
{
    public int Amount { get; set; }
    public string Currency { get; set; }
    public IList<CartItem> Items { get; set; }
}