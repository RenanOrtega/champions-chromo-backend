using ChampionsChromo.Application.Albums.Commands.DeleteAlbum;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Schools.Commands.CreateSchool;

public class DeleteAlbumCommandHandler(IAlbumRepository albumRepository) : IRequestHandler<DeleteAlbumCommand, Result>
{
    private readonly IAlbumRepository _albumRepository = albumRepository;

    public async Task<Result> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _albumRepository.DeleteAsync(request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failure to delete school: {ex.Message}");
        }
    }
}
