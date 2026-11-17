using FluentValidation;
using MongoDB.Bson;

namespace ChampionsChromo.Application.Orders.Commands.ArchiveOrders;

public class ArchiveOrdersCommandValidator : AbstractValidator<ArchiveOrdersCommand>
{
    public ArchiveOrdersCommandValidator()
    {
        RuleFor(c => c.Ids)
            .NotEmpty().WithMessage("Informe ao menos um pedido.")
            .Must(ids => ids.Count <= 1000).WithMessage("Máximo de 1000 pedidos por vez.");
        RuleForEach(c => c.Ids)
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("ID de pedido inválido.");
    }
}
