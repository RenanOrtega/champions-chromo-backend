using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Schools.Queries.GetSchools;

public class GetSchoolsQueryHandler(ISchoolRepository schoolRepository, IMapper mapper) : IRequestHandler<GetSchoolsQuery, Result<IEnumerable<SchoolDto>>>
{
    private readonly ISchoolRepository _schoolRepository = schoolRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<SchoolDto>>> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
    {
        var schools = await _schoolRepository.GetAllAsync();

        return Result<IEnumerable<SchoolDto>>.Success(_mapper.Map<IEnumerable<SchoolDto>>(schools));
    }
}
