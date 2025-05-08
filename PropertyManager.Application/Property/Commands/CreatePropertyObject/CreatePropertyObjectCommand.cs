using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Interfaces.Services;
using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Common;
using System.Net;

namespace PropertyManager.Application.Property.Commands.CreatePropertyObject;

public class CreatePropertyObjectCommand : IRequest<TResponse>
{
    public PropertyModel? property { get; set; }
}

public class CreatePropertyObjectCommandHandler : IRequestHandler<CreatePropertyObjectCommand, TResponse>
{
    private readonly ILogger<CreatePropertyObjectCommandHandler> _logger;
    private readonly IPropertyObjectRepository _propertyObjectRepository;
    private readonly IPropertyTraceObjectService _propertyTraceObjectService;
    private readonly IOwnerObjectRepository _ownerObjectRepository;
    private readonly IMapper _autoMapper;
    private OwnerModelOut ownerOut = null!;
    private PropertyModelOut propertyOut = null!;
    public CreatePropertyObjectCommandHandler(
        IPropertyObjectRepository propertyObjectRepository,
        IPropertyTraceObjectService propertyTraceObjectService,
        IOwnerObjectRepository ownerObjectRepository,
        ILogger<CreatePropertyObjectCommandHandler> logger,
        IMapper autoMapper)
    {
        _propertyObjectRepository = propertyObjectRepository;
        _propertyTraceObjectService = propertyTraceObjectService;
        _ownerObjectRepository = ownerObjectRepository;
        _logger = logger;
        _autoMapper = autoMapper;
    }
    public async Task<TResponse> Handle(CreatePropertyObjectCommand request, CancellationToken cancellationToken)
    {
        var statusCode = HttpStatusCode.OK;

        var message = "The process was carried out correctly.";

        var result = "NoChange";

        try
        {
            var ownerSearch = await _ownerObjectRepository.GetOwnerByIdAsync(request.property!.IdOwner);
            if (ownerSearch is null)
            {
                return new TResponse()
                {
                    Message = "Owner Not Found.",
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new List<Error> { new() { Message = $"The Id Owner {request.property!.IdOwner} was not Found.", Field = "IdOwner" } }
                };
            }
            var property = _autoMapper.Map<PropertyManager.Domain.Entities.Property>(request.property);
            property.CreatedBy = "aaraujo";
            property.CreatedDate = DateTime.Now;
            property.UpdatedDate = DateTime.Now;
            var idTransaction = await _propertyObjectRepository.AddPropertyAsync(property);
            if (idTransaction > 0)
            {
                
                var propertySearch = await _propertyObjectRepository.GetPropertyByIdAsync(idTransaction);

                propertyOut = _autoMapper.Map<PropertyModelOut>(propertySearch);

                var ownerTransaction = await _ownerObjectRepository.GetOwnerByIdAsync(request.property.IdOwner);
                if (ownerTransaction is not null)
                {
                    ownerOut = new OwnerModelOut
                    {
                        Address = ownerTransaction.Address,
                        Birthday = ownerTransaction.Birthday,
                        IdOwner = ownerTransaction.IdOwner,
                        Name = ownerTransaction.Name,
                        Photo = ownerTransaction.Photo

                    };

                    propertyOut.Owner = ownerOut;
                }
                var listpropertyTraces = await _propertyTraceObjectService.GetByPropertyTraceByIdAsync(idTransaction);
                propertyOut.PropertyTraces = listpropertyTraces.propertyTraces;

            }
            


        }
        catch (Exception e)
        {
            return new TResponse()
            {
                Message = "Errors have occurred.",
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<Error> { new() { Message = e.Message, Field = "Property" } }
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
            Data = propertyOut
        };
    }
}

