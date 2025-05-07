using FluentValidation;
using PropertyManager.Application.Common.Models;

namespace PropertyManager.Application.Property.Commands.Validations;

public class PropertyUpdateValidator : AbstractValidator<PropertyUpdateModel>
{
    public PropertyUpdateValidator()
    {

        RuleFor(p => p.IdProperty)
            .GreaterThan(0).WithMessage("Property Id must be a positive number.");

        RuleFor(p => p.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.CodeInternal)
            .NotEmpty().WithMessage("Internal code is required.")
            .MaximumLength(50).WithMessage("Internal code must not exceed 50 characters.");

        RuleFor(p => p.Year)
            .InclusiveBetween(1800, DateTime.Now.Year).WithMessage($"Year must be between 1800 and {DateTime.Now.Year}.");

        RuleFor(p => p.IdOwner)
            .GreaterThan(0).WithMessage("Owner Id must be a positive number.");

    }
}

