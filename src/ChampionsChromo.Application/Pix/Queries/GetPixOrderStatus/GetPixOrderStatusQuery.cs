using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Pix.Queries.GetPixOrderStatus;

public record GetPixOrderStatusQuery(string IntegrationId) : IRequest<Result<PixOrderStatus>>;
