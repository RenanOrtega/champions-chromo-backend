using FluentValidation;

namespace ChampionsChromo.Application.Schools.Commands.CreateSchool;

public class CreateSchoolValidator : AbstractValidator<CreateSchoolCommand>
{
    public CreateSchoolValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(v => v.City)
            .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

        RuleFor(v => v.State)
            .MaximumLength(50).WithMessage("State must not exceed 50 characters.");
    }
}
