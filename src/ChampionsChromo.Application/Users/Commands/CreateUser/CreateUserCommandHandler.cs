using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Repositories.Interfaces;
using MediatR;

namespace ChampionsChromo.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User
        {
            GoogleId = request.GoogleId,
            Email = request.Email,
            Name = request.Name,
            PhotoUrl = request.PhotoUrl
        };

        try
        {
            await _userRepository.AddAsync(entity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create album: {ex.Message}");
        }
    }
}
