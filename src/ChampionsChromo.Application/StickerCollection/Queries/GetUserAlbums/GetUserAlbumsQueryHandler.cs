using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbums;

public class GetUserAlbumsQueryHandler(IUserAlbumRepository userAlbumRepository, IMapper mapper) : IRequestHandler<GetUserAlbumsQuery, Result<IEnumerable<UserAlbumDto>>>
{
    private readonly IUserAlbumRepository _userAlbumRepository = userAlbumRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<UserAlbumDto>>> Handle(GetUserAlbumsQuery request, CancellationToken cancellationToken)
    {
        var userAlbums = await _userAlbumRepository.GetAllAsync();

        return Result<IEnumerable<UserAlbumDto>>.Success(_mapper.Map<IEnumerable<UserAlbumDto>>(userAlbums));
    }
}
