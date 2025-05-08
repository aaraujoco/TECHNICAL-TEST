using FluentValidation;
using PropertyManager.Application.Common.Models;

namespace PropertyManager.Application.Property.Commands.Validations
{
    public class UploadPropertyImageModelValidator : AbstractValidator<UploadPropertyImageModel>
    {
        public UploadPropertyImageModelValidator() {

            RuleFor(x => x.IdProperty)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0).WithMessage("Property Id must be a positive number.");

            RuleFor(x => x.File)
                .NotEmpty()
                .NotNull();
        }
    }
}
