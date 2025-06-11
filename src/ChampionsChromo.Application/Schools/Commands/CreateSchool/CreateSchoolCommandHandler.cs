using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Schools.Commands.CreateSchool;

public class CreateSchoolCommandHandler(ISchoolRepository schoolRepository) : IRequestHandler<CreateSchoolCommand, Result>
{
    private readonly ISchoolRepository _schoolRepository = schoolRepository;

    public async Task<Result> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = new School
        {
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            Email = request.Email,
            Phone = request.Phone,
            State = request.State,
            BgWarningColor = request.BgWarningColor,
            Warning = request.Warning
        };

        try
        {
            await _schoolRepository.AddAsync(entity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failure to create school: {ex.Message}");
        }
    }
}
