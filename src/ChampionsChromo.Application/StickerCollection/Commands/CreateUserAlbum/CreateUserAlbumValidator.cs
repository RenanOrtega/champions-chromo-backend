using FluentValidation;

namespace ChampionsChromo.Application.StickerCollection.Commands.CreateUserAlbum;

public class CreateUserAlbumValidator : AbstractValidator<CreateUserAlbumCommand>
{
    public CreateUserAlbumValidator()
    {
        RuleFor(u => u.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
