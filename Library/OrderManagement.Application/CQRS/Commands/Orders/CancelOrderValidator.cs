using FluentValidation;

namespace OrderManagement.Application.CQRS.Commands.Orders;

public class CancelOrderValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order Id is required.");
    }
}
