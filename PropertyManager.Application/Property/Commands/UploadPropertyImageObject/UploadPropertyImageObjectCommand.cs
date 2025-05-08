using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Property.Commands.UpdatePropertyObject;
using PropertyManager.Domain.Common;

namespace PropertyManager.Application.Property.Commands.UploadPropertyImageObject
{
    public class UploadPropertyImageObjectCommand : IRequest<TResponse>
    {
        public UploadPropertyImageModel propertyImage { get; set; }
    }

    public class UploadPropertyImageObjectCommandHandler : IRequestHandler<UploadPropertyImageObjectCommand, TResponse>
    {
        private readonly ILogger<UpdatePropertyObjectCommand> _logger;
        private readonly IPropertyObjectRepository _propertyObjectRepository;
        private readonly IPropertyImageObjectRepository _propertyImageObjectRepository;
        private readonly IMapper _autoMapper;

        public UploadPropertyImageObjectCommandHandler(
            ILogger<UpdatePropertyObjectCommand> logger,
            IPropertyObjectRepository propertyObjectRepository,
            IPropertyImageObjectRepository propertyImageObjectRepository,
            IMapper autoMapper)
        {
            _logger = logger;
            _propertyObjectRepository = propertyObjectRepository;
            _propertyImageObjectRepository = propertyImageObjectRepository;
            _autoMapper = autoMapper;
        }


        public async Task<TResponse> Handle(UploadPropertyImageObjectCommand request, CancellationToken cancellationToken)
        {
            var statusCode = HttpStatusCode.OK;

            var message = "The process was carried out correctly.";

            var result = "NoChange";

            try
            {
                var propertySearch = await _propertyObjectRepository.GetPropertyByIdAsync(request.propertyImage.IdProperty);

                if (propertySearch is null)
                {
                    return new TResponse()
                    {
                        Message = "Property Not Found.",
                        StatusCode = HttpStatusCode.NotFound,
                        Errors = new List<Domain.Common.Error> { new() { Message = string.Empty, Field = string.Empty } }
                    };
                }

                await _propertyImageObjectRepository.AddPropertyImageAsync(request.propertyImage);
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
                Data = request.propertyImage
            };
        }
    }
}
