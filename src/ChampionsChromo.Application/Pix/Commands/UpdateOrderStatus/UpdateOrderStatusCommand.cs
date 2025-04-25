using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Enums;
using MediatR;

namespace ChampionsChromo.Application.Pix.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand(string integrationId, PixStatus status) : IRequest<Result>
{
    public string IntegrationId { get; set; } = integrationId;
    public PixStatus Status { get; set; } = status;
}
