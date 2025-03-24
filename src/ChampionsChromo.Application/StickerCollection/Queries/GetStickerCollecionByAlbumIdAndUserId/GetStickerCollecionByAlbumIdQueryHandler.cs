using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;

public class GetStickerCollecionByAlbumIdAndUserIdQueryHandler(IUserAlbumRepository userAlbumRepository, IMapper mapper) : IRequestHandler<GetStickerCollecionByAlbumIdAndUserIdQuery, Result<IEnumerable<UserAlbumDto>>>
{
    private readonly IUserAlbumRepository _userAlbumRepository = userAlbumRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<UserAlbumDto>>> Handle(GetStickerCollecionByAlbumIdAndUserIdQuery request, CancellationToken cancellationToken)
    {
        var userAlbum = await _userAlbumRepository.FindByAlbumIdAsync(request.AlbumId, request.UserId);

        if (userAlbum == null)
            return Result<IEnumerable<UserAlbumDto>>.Failure($"StickerCollection with AlbumId {request.AlbumId} and UserId {request.UserId} not found.");

        return Result<IEnumerable<UserAlbumDto>>.Success(_mapper.Map<IEnumerable<UserAlbumDto>>(userAlbum));
    }
}
