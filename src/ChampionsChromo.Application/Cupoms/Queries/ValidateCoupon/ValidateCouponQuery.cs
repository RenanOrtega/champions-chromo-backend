using MediatR;

namespace ChampionsChromo.Application.Cupoms.Queries.ValidateCoupon;

public record ValidateCouponQuery(string Code) : IRequest<ValidateCouponResponse>;
