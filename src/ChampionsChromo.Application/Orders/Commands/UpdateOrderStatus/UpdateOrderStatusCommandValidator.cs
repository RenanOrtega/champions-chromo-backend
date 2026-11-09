using FluentValidation;

namespace ChampionsChromo.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(c => c.Status).IsInEnum().WithMessage("Status inválido.");
        RuleFor(c => c.Note).MaximumLength(500);
    }
}
