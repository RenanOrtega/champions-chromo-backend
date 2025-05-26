using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Albums.Commands.CreateAlbum;

public class CreateAlbumCommandHandler(IAlbumRepository albumRepository) : IRequestHandler<CreateAlbumCommand, Result>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;

    public async Task<Result> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = new Album
        {
            SchoolId = request.SchoolId,
            Name = request.Name,
            Price = request.Price,
            ReleaseDate = request.ReleaseDate,
            CoverImage = request.CoverImage,
            TotalStickers = request.TotalStickers,
            HasA4 = request.HasA4,
            HasCommon = request.HasCommon,
            HasLegend = request.HasLegend
        };

        try
        {
            await _albumRepository.AddAsync(entity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create album: {ex.Message}");
        }
    }
}
