using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Cupoms.Queries.GetCupoms;

public class GetCupomsQueryHandler(ICupomRepository cupomRepository, IMapper mapper) : IRequestHandler<GetCupomsQuery, Result<IEnumerable<CupomDto>>>
{
    private readonly ICupomRepository _cupomRepository = cupomRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<CupomDto>>> Handle(GetCupomsQuery request, CancellationToken cancellationToken)
    {
        var albums = await _cupomRepository.GetAllAsync();

        return Result<IEnumerable<CupomDto>>.Success(_mapper.Map<IEnumerable<CupomDto>>(albums));
    }
}
