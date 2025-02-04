using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Schools.Queries.GetSchoolById;

public class GetSchoolByIdQueryHandler(ISchoolRepository schoolRepository, IMapper mapper) : IRequestHandler<GetSchoolByIdQuery, Result<SchoolDto>>
{
    private readonly ISchoolRepository _schoolRepository = schoolRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<SchoolDto>> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
    {
        var school = await _schoolRepository.GetByIdAsync(request.Id);

        if (school == null)
            return Result<SchoolDto>.Failure($"School with ID {request.Id} not found.");

        return Result<SchoolDto>.Success(_mapper.Map<SchoolDto>(school));
    }
}
