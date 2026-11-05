using FluentValidation;

namespace ChampionsChromo.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(o => o.Albums)
            .NotEmpty().WithMessage("O pedido precisa ter ao menos um álbum.")
            .Must(a => a.Count <= 50).WithMessage("Pedido com álbuns demais.");

        RuleForEach(o => o.Albums).ChildRules(album =>
        {
            album.RuleFor(a => a.AlbumId).NotEmpty().WithMessage("Álbum inválido.");

            album.RuleFor(a => a.Stickers)
                .NotEmpty().WithMessage("Escolha ao menos uma figurinha.")
                .Must(s => s.Count <= 2000).WithMessage("Figurinhas demais em um álbum.");

            album.RuleForEach(a => a.Stickers).ChildRules(sticker =>
            {
                sticker.RuleFor(s => s.Type).IsInEnum().WithMessage("Tipo de figurinha inválido.");
                sticker.RuleFor(s => s.Quantity).InclusiveBetween(1, 100).WithMessage("Quantidade inválida.");
                sticker.RuleFor(s => s.Number).MaximumLength(20).WithMessage("Número de figurinha inválido.");
            });
        });

        RuleFor(o => o.CouponCode).MaximumLength(50);

        RuleFor(o => o.Customer).NotNull().WithMessage("Informe seus dados.");

        When(o => o.Customer is not null, () =>
        {
            RuleFor(o => o.Customer!.Name)
                .NotEmpty().WithMessage("Informe seu nome.")
                .MaximumLength(150);

            RuleFor(o => o.Customer!.Email)
                .NotEmpty().WithMessage("Informe seu email.")
                .EmailAddress().WithMessage("Email inválido.")
                .MaximumLength(150);

            When(o => o.Customer!.Address is not null, () =>
            {
                RuleFor(o => o.Customer!.Address!.Street).MaximumLength(200);
                RuleFor(o => o.Customer!.Address!.Number).MaximumLength(20);
                RuleFor(o => o.Customer!.Address!.Neighborhood).MaximumLength(100);
                RuleFor(o => o.Customer!.Address!.PostalCode).MaximumLength(20);
                RuleFor(o => o.Customer!.Address!.City).MaximumLength(100);
                RuleFor(o => o.Customer!.Address!.State).MaximumLength(50);
                RuleFor(o => o.Customer!.Address!.Complement).MaximumLength(200);
            });
        });
    }
}
