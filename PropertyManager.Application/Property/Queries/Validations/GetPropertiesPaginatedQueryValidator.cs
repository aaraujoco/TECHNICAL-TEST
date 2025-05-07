using FluentValidation;

namespace PropertyManager.Application.Property.Queries.Validations
{
    public class GetPropertiesPaginatedQueryValidator : AbstractValidator<GetPropertiesPaginatedQuery>
    {
        public GetPropertiesPaginatedQueryValidator()
        {
            RuleFor(x => x.Price)
           .Must(BeAValidDecimal).WithMessage("Price must be a valid number.")
           .Must(BeLessOrEqualZero).WithMessage("Price must be less than or equal to zero.");
        

            RuleFor(x => x.Year)
           .Must(BeAValidNumber).WithMessage("Year must be a valid number.")
           .Must(BeLessOrEqualZeroNumber).WithMessage("Year must be less than or equal to zero.");

            RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("The value of 'page' must be greater than 0.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("The value of 'size' must be greater than 0.");
        }


        /// <summary>
        /// Verifica que el string se pueda convertir a un número decimal válido.
        /// </summary>
        private bool BeAValidDecimal(decimal price)
        {
            string priceString = price.ToString();
            decimal parsed;
            return decimal.TryParse(priceString, out parsed);
        }

        /// <summary>
        /// Verifica que el número sea menor o igual a cero.
        /// </summary>
        private bool BeLessOrEqualZero(decimal price)
        {
            string priceString = price.ToString();
            decimal parsed;
            if (decimal.TryParse(priceString, out parsed))
            {
                return parsed <= 0;
            }
            return false;
        }

        private bool BeAValidNumber(int price)
        {
            string priceString = price.ToString();
            int parsed;
            return int.TryParse(priceString, out parsed);
        }

        /// <summary>
        /// Verifica que el número sea menor o igual a cero.
        /// </summary>
        private bool BeLessOrEqualZeroNumber(int price)
        {
            string priceString = price.ToString();
            int parsed;
            if (int.TryParse(priceString, out parsed))
            {
                return parsed <= 0;
            }
            return false;
        }

    }
}
