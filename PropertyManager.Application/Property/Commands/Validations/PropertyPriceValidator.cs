using FluentValidation;
using PropertyManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Property.Commands.Validations;

    public class PropertyPriceValidator : AbstractValidator<PropertyPriceModel>
    {
        public PropertyPriceValidator()
        {
            RuleFor(x => x.IdProperty)
                .GreaterThan(0).WithMessage("Property Id must be a positive number.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");


        }
    }

