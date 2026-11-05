using AutoMapper;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Queries.ValidateCoupon;

public class ValidateCouponQueryHandler(ICupomRepository cupomRepository, IMapper mapper) : IRequestHandler<ValidateCouponQuery, ValidateCouponResponse>
{
    private readonly ICupomRepository _cupomRepository = cupomRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ValidateCouponResponse> Handle(ValidateCouponQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _cupomRepository.FindByCodeAsync(request.Code);

        if (coupon is null)
        {
            return new ValidateCouponResponse
            {
                Coupon = null,
                Message = "Cupom não encontrado."
            };
        }

        var reason = CouponRules.GetUnavailableReason(coupon, DateTime.UtcNow);
        if (reason is not null)
        {
            return new ValidateCouponResponse
            {
                Coupon = null,
                Message = reason
            };
        }

        return new ValidateCouponResponse
        {
            Coupon = _mapper.Map<CupomDto>(coupon),
            Message = "Coupon validado com sucesso."
        };
    }
}
