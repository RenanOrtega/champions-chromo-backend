using FluentValidation;

namespace ChampionsChromo.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {

    }
}
