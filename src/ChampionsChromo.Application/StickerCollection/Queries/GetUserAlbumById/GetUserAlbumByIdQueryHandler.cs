using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbumById;

public class GetUserAlbumByIdQueryHandler(IUserAlbumRepository userAlbumRepository, IMapper mapper) : IRequestHandler<GetUserAlbumByIdQuery, Result<UserAlbumDto>>
{
    private readonly IUserAlbumRepository _userAlbumRepository = userAlbumRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<UserAlbumDto>> Handle(GetUserAlbumByIdQuery request, CancellationToken cancellationToken)
    {
        var userAlbum = await _userAlbumRepository.GetByIdAsync(request.Id);

        if (userAlbum == null)
            return Result<UserAlbumDto>.Failure($"UserAlbum with ID {request.Id} not found.");

        return Result<UserAlbumDto>.Success(_mapper.Map<UserAlbumDto>(userAlbum));
    }
}
