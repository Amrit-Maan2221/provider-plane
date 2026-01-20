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
        RuleFor(x => x.Contacts)
            .NotEmpty().WithMessage("At least one contact is required.");

        RuleFor(x => x.Contacts.Count(c => c.IsPrimary))
            .Equal(1)
            .WithMessage("Exactly one primary contact is required.");
    }
}
