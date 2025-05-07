using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Common;
using System.Net;

namespace PropertyManager.Application.Property.Commands.UpdatePropertyObject;

public class UpdatePropertyObjectCommand : IRequest<TResponse>
{
    public PropertyUpdateModel? property { get; set; }
}
public class UpdatePropertyObjectCommandHandler : IRequestHandler<UpdatePropertyObjectCommand, TResponse>
{
    private readonly ILogger<UpdatePropertyObjectCommand> _logger;
    private readonly IPropertyObjectRepository _propertyObjectRepository;
    private readonly IOwnerObjectRepository _ownerObjectRepository;
    private readonly IMapper _autoMapper;

    public UpdatePropertyObjectCommandHandler(
        IPropertyObjectRepository propertyObjectRepository,
        IOwnerObjectRepository ownerObjectRepository,
        ILogger<UpdatePropertyObjectCommand> logger,
        IMapper autoMapper)
    {
        _propertyObjectRepository = propertyObjectRepository;
        _ownerObjectRepository = ownerObjectRepository;
        _logger = logger;
        _autoMapper = autoMapper;
    }
    public async Task<TResponse> Handle(UpdatePropertyObjectCommand request, CancellationToken cancellationToken)
    {
        var statusCode = HttpStatusCode.OK;

        var message = "The process was carried out correctly.";

        var result = "NoChange";

        try
        {

            var propertySearch = await _propertyObjectRepository.GetPropertyByIdAsync(request.property.IdProperty);
            
            if (propertySearch is null )
            {
                return new TResponse()
                {
                    Message = "Property Not Found.",
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new List<Domain.Common.Error> { new() { Message = string.Empty, Field = string.Empty } }
                };
            }
            var ownerSearch = await _ownerObjectRepository.GetOwnerByIdAsync(request.property.IdOwner);
            if (ownerSearch is null)
            {
                return new TResponse()
                {
                    Message = "Owner Not Found.",
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new List<Domain.Common.Error> { new() { Message = string.Empty, Field = string.Empty } }
                };
            }
            var property = _autoMapper.Map<PropertyManager.Domain.Entities.Property>(request.property);
            property.CreatedBy = "aaraujo";
            property.UpdatedDate = DateTime.Now;
            await _propertyObjectRepository.PropertyUpdateModel(property);
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
        statusCode = HttpStatusCode.OK;
        message = "The property has been updated successfully.";
        result = "Update";

        return new TResponse()
        {
            Result = result,
            Message = message,
            StatusCode = statusCode,
            Data = request.property
        };
    }
}
