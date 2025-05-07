using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PropertyManager.Application.Property.Queries.Validations
{
    public class GetPropertyTraceByPropertyIdQueryValidator : AbstractValidator<GetPropertyTraceByPropertyIdQuery>
    {
        public GetPropertyTraceByPropertyIdQueryValidator()
        {
            RuleFor(p => p.IdProperty)
           .GreaterThan(0).WithMessage("Property Id must be a positive number.");
        }
    }
}
