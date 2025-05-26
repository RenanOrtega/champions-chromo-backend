using ChampionsChromo.Application.Albums.Commands.UpdateAlbum;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Application.Schools.Commands.UpdateSchool;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Albums.Commands.CreateAlbum;

public class UpdateAlbumCommandHandler(IAlbumRepository albumRepository) : IRequestHandler<UpdateAlbumCommmand, Result>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;

    public async Task<Result> Handle(UpdateAlbumCommmand request, CancellationToken cancellationToken)
    {
        var updateAlbumDto = new UpdateAlbumDto
        {
            Name = request.Name,
            CoverImage = request.CoverImage,
            HasCommon = request.HasCommon,
            HasLegend = request.HasLegend,
            HasA4 = request.HasA4,
            TotalStickers = request.TotalStickers
        };

        try
        {
            await _albumRepository.UpdateAsync(request.Id, updateAlbumDto);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create album: {ex.Message}");
        }
    }
}
