using ChampionsChromo.Application.Albums.Commands.UpdateAlbum;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Schools.Commands.UpdateSchool;

public class UpdateSchoolCommandHandler(ISchoolRepository schoolRepository) : IRequestHandler<UpdateSchoolCommand, Result>
{
    private readonly ISchoolRepository _schoolRepository = schoolRepository;

    public async Task<Result> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
    {
        var updateAlbumDto = new UpdateSchoolDto
        {
            Name = request.Name,
            City = request.City,
            State = request.State,
            Warning = request.Warning,
            BgWarningColor = request.BgWarningColor,
            ShippingCost = request.ShippingCost  
        };

        try
        {
            await _schoolRepository.UpdateAsync(request.Id, updateAlbumDto);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to update school: {ex.Message}");
        }
    }
}
