using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Commands.CreateUserAlbum;

public class CreateUserAlbumCommandHandler(IUserAlbumRepository userAlbumRepository) : IRequestHandler<CreateUserAlbumCommand, Result>
{
    private readonly IUserAlbumRepository _userAlbumRepository = userAlbumRepository;

    public async Task<Result> Handle(CreateUserAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = new UserAlbum
        {
            Albums = request.Albums,
            UserId = request.UserId,
        };

        try
        {
            await _userAlbumRepository.AddAsync(entity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failure to create UserAlbum: {ex.Message}");
        }
    }
}
