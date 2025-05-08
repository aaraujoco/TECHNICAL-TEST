using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Common;
using PropertyManager.Domain.Entities;

namespace PropertyManager.Application.Property.Commands.CreatePropertyTraceObject
{
    public class CreatePropertyTraceObjectCommand : IRequest<TResponse>
    {
        public PropertyTraceModel? propertyTrace { get; set; }
    }

    public class CreatePropertyTraceObjectCommandHandler : IRequestHandler<CreatePropertyTraceObjectCommand, TResponse>
    {
        private readonly ILogger<CreatePropertyTraceObjectCommandHandler> _logger;
        private readonly IPropertyTraceObjectRepository _propertyTraceObjectRepository;
        private readonly IPropertyObjectRepository _propertyObjectRepository;
        private readonly IMapper _autoMapper;
        public CreatePropertyTraceObjectCommandHandler(
            IPropertyTraceObjectRepository propertyTraceObjectRepository,
            IPropertyObjectRepository propertyObjectRepository,
            ILogger<CreatePropertyTraceObjectCommandHandler> logger,
            IMapper autoMapper)
        {
            _propertyTraceObjectRepository = propertyTraceObjectRepository;
            _propertyObjectRepository = propertyObjectRepository;
            _logger = logger;
            _autoMapper = autoMapper;
        }
        public async Task<TResponse> Handle(CreatePropertyTraceObjectCommand request, CancellationToken cancellationToken)
        {
            var statusCode = HttpStatusCode.OK;

            var message = "The process was carried out correctly.";

            var result = "NoChange";

            try
            {
                var propertySearch = await _propertyObjectRepository.GetPropertyByIdAsync(request.propertyTrace!.IdProperty);
                if (propertySearch is null)
                {
                    return new TResponse()
                    {
                        Message = "Property Not Found.",
                        StatusCode = HttpStatusCode.NotFound,
                        Errors = new List<Error> { new() { Message = $"The Id Property {request.propertyTrace.IdProperty} was not Found.", Field = "IdProperty" } }
                    };
                }
                var property = _autoMapper.Map<PropertyTrace>(request.propertyTrace);
                property.CreatedBy = "aaraujo";
                property.CreatedDate = DateTime.Now;
                property.UpdatedDate = DateTime.Now;
                await _propertyTraceObjectRepository.CreateAsync(property);
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
            message = "The property Trace has been created successfully.";
            result = "Created";

            return new TResponse()
            {
                Result = result,
                Message = message,
                StatusCode = statusCode,
                Data = request.propertyTrace
            };
        }
    }


}
