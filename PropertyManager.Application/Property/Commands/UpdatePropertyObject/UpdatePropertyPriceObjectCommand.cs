using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Common;
using System.Net;

namespace PropertyManager.Application.Property.Commands.UpdatePropertyObject;
    public class UpdatePropertyPriceObjectCommand : IRequest<TResponse>
    {
        public PropertyPriceModel? propertyPrice { get; set; }
    }
public class UpdatePropertyPriceObjectCommandHandler : IRequestHandler<UpdatePropertyPriceObjectCommand, TResponse>
{
    private readonly ILogger<UpdatePropertyPriceObjectCommandHandler> _logger;
    private readonly IPropertyObjectRepository _propertyObjectRepository;

    public UpdatePropertyPriceObjectCommandHandler(
        IPropertyObjectRepository propertyObjectRepository,
        ILogger<UpdatePropertyPriceObjectCommandHandler> logger)
    {
        _propertyObjectRepository = propertyObjectRepository;
        _logger = logger;
    }
    public async Task<TResponse> Handle(UpdatePropertyPriceObjectCommand request, CancellationToken cancellationToken)
    {
        var statusCode = HttpStatusCode.OK;

        var message = "The process was carried out correctly.";

        var result = "NoChange";

        try
        {
            var property = await _propertyObjectRepository.GetPropertyByIdAsync(request.propertyPrice.IdProperty);

            if (property is null)
            {
                return new TResponse()
                {
                    Message = "Property Not Found.",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            await _propertyObjectRepository.ChangePropertyPriceAsync(request.propertyPrice.IdProperty, request.propertyPrice.Price, "aaraujo");
        }
        catch (Exception e)
        {
            return new TResponse()
            {
                Message = "Errors have occurred.",
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<Domain.Common.Error> { new() { Message = e.Message, Field = "" } }
            };
        }
        statusCode = HttpStatusCode.Created;
        message = "The property has been created successfully.";
        result = "Created";

        return new TResponse()
        {
            Result = result,
            Message = message,
            StatusCode = statusCode,
            Data = request.propertyPrice
        };
        throw new NotImplementedException();
    }
}

