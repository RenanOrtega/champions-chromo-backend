using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Queries.GetCupomById;

public class GetCupomByIdQueryHandler(ICupomRepository cupomRepository, IMapper mapper) : IRequestHandler<GetCupomByIdQuery, Result<CupomDto>>
{
    private readonly ICupomRepository _cupomRepository = cupomRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CupomDto>> Handle(GetCupomByIdQuery request, CancellationToken cancellationToken)
    {
        var album = await _cupomRepository.GetByIdAsync(request.Id);

        if (album == null)
            return Result<CupomDto>.Failure($"Cupom with ID {request.Id} not found.");

        return Result<CupomDto>.Success(_mapper.Map<CupomDto>(album));
    }
}
