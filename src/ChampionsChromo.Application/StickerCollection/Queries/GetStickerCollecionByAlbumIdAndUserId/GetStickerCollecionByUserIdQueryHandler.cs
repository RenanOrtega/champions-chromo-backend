using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;

public class GetStickerCollecionByUserIdQueryHandler(IUserAlbumRepository userAlbumRepository, IMapper mapper) : IRequestHandler<GetStickerCollecionByUserIdQuery, Result<UserAlbumDto>>
{
    private readonly IUserAlbumRepository _userAlbumRepository = userAlbumRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<UserAlbumDto>> Handle(GetStickerCollecionByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userAlbum = await _userAlbumRepository.GetByUserIdAsync(request.UserId);

        if (userAlbum == null)
        {
            var defaultUserAlbum = new UserAlbum()
            {
                UserId = request.UserId,
                Albums = [], 
            };
            await _userAlbumRepository.AddAsync(defaultUserAlbum);
            return Result<UserAlbumDto>.Success(_mapper.Map<UserAlbumDto>(defaultUserAlbum));
        }

        return Result<UserAlbumDto>.Success(_mapper.Map<UserAlbumDto>(userAlbum));
    }
}
