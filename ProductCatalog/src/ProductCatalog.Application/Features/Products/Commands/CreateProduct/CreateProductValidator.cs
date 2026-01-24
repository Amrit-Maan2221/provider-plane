using FluentValidation;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Category)
            .NotEmpty()
            .MaximumLength(100);
    }
}
