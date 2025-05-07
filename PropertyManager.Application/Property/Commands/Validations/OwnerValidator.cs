using FluentValidation;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Utilities;

namespace PropertyManager.Application.Property.Commands.Validations;
public class OwnerValidator : AbstractValidator<OwnerModel>
{
    public OwnerValidator()
    {
        RuleFor(model => model.Name)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(50)
            .NotEmpty()
            .NotNull();

        RuleFor(model => model.Address)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(50)
            .NotEmpty()
            .NotNull();

        RuleFor(model => model.Birthday)
           .Cascade(CascadeMode.Stop)
           .Must(UtilDate.BeValidDate)
           .When(x => !string.IsNullOrEmpty(x.Birthday));
    }
}

