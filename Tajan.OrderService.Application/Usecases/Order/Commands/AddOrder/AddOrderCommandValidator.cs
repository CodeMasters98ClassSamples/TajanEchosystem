using FluentValidation;

namespace Tajan.OrderService.Application.Usecases;

public class AddOrderCommandValidator : AbstractValidator<AddOrderCommand>
{
    public AddOrderCommandValidator() {
        RuleFor(p => p.Produts)
            .NotEmpty()
            .WithMessage("Amount is required.");
    }

}
