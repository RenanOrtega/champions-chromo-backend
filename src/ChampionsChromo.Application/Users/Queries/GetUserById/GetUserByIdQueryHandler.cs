using AutoMapper;
using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null)
            return Result<UserDto>.Failure($"User with ID {request.Id} not found.");

        return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
    }
}