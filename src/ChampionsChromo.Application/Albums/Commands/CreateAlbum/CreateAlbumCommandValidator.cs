using FluentValidation;

namespace ChampionsChromo.Application.Albums.Commands.CreateAlbum;

public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
{
    public CreateAlbumCommandValidator()
    {
        RuleFor(v => v.SchoolId)
            .NotEmpty().WithMessage("SchoolId is required");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(v => v.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");

        RuleFor(v => v.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required");
    }
}
