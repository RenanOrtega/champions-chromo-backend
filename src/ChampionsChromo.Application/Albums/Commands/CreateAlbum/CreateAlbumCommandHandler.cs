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
            CommonStickers = request.CommonStickers,
            FrameStickers = request.FrameStickers,
            LegendStickers = request.LegendStickers,
            A4Stickers = request.A4Stickers,
            ReleaseDate = request.ReleaseDate,
            CoverImage = request.CoverImage
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
