using FluentValidation;
using TenantRegistry.API.DTOs;

public class CreateTenantRequestValidator : AbstractValidator<CreateTenantRequest>
{
    public CreateTenantRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Slug)
            .NotEmpty()
            .Matches("^[a-z0-9-]+$");

        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.Timezone).NotEmpty();
    }
}
