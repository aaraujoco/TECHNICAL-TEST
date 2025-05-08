using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Utilities;

namespace PropertyManager.Application.Property.Commands.Validations
{
    public class PropertyTraceValidator : AbstractValidator<PropertyTraceModel>
    {
        public PropertyTraceValidator()
        {
            RuleFor(model => model.DateSale)
            .Cascade(CascadeMode.Stop)
            .Must(UtilDate.BeValidDate)
            .When(x => !string.IsNullOrEmpty(x.DateSale));

            RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.Value)
           .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(p => p.IdProperty)
            .GreaterThan(0).WithMessage("Property Id must be a positive number.");

        }
    }
}
