using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PropertyManager.Application.Property.Commands.Validations;
using PropertyManager.Application.Property.Queries.Validations;

namespace PropertyManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddValidatorsFromAssemblyContaining<OwnerValidator>();
            services.AddValidatorsFromAssemblyContaining<PropertyValidator>();
            services.AddValidatorsFromAssemblyContaining<PropertyPriceValidator>();
            services.AddValidatorsFromAssemblyContaining<PropertyUpdateValidator>();
            services.AddValidatorsFromAssemblyContaining<GetPropertiesPaginatedQueryValidator>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            return services;
        }
    }
}
