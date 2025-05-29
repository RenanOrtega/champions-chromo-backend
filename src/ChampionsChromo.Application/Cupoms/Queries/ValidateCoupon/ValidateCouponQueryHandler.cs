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

        if (coupon.UsageLimit > 0 && coupon.UsedCount >= coupon.UsageLimit)
        {
            return new ValidateCouponResponse
            {
                Coupon = null,
                Message = "Cupom esgotado."
            };
        }

        //if (coupon.ExpiresAt < DateTime.UtcNow)
        //{
        //    return new ValidateCouponResponse { Coupon = null, Message = "Coupon has expired." };
        //}

        return new ValidateCouponResponse
        {
            Coupon = _mapper.Map<CupomDto>(coupon),
            Message = "Coupon validado com sucesso."
        };
    }
}
