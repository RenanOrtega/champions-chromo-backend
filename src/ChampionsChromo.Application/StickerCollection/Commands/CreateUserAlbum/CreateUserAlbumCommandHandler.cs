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
        try
        {
            var userAlbum = await _userAlbumRepository.GetByUserIdAsync(request.UserId);

            if (userAlbum is null)
            {
                var entity = new UserAlbum
                {
                    Albums = request.Albums,
                    UserId = request.UserId,
                };
                await _userAlbumRepository.AddAsync(entity);
                return Result.Success();
            }

            userAlbum.Albums = request.Albums;
            await _userAlbumRepository.UpdateAsync(request.UserId, userAlbum);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failure to create UserAlbum: {ex.Message}");
        }
    }
}
