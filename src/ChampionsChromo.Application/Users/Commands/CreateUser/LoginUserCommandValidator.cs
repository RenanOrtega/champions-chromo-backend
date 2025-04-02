using FluentValidation;

namespace ChampionsChromo.Application.Users.Commands.CreateUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        
    }
}
