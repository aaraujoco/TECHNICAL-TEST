using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Property.Commands.UpdatePropertyObject;
using PropertyManager.Domain.Common;

namespace PropertyManager.Application.Property.Queries
{
    public class GetPropertyTraceByPropertyIdQuery : IRequest<TResponse>
    {
        public int IdProperty { get; set; }
    }

    public sealed class GetPropertyTraceByPropertyIdQueryHandler : IRequestHandler<GetPropertyTraceByPropertyIdQuery, TResponse>
    {
        private readonly ILogger<GetPropertyTraceByPropertyIdQueryHandler> _logger;
        private readonly IPropertyTraceObjectRepository _propertyTraceObjectRepository;
        private readonly IMapper _autoMapper;

        public GetPropertyTraceByPropertyIdQueryHandler(IPropertyTraceObjectRepository propertyTraceObjectRepository,
            ILogger<GetPropertyTraceByPropertyIdQueryHandler> logger,
            IMapper autoMapper)
        {
            _propertyTraceObjectRepository = propertyTraceObjectRepository;
            _logger = logger;
            _autoMapper = autoMapper;
        }

        public async Task<TResponse> Handle(GetPropertyTraceByPropertyIdQuery request, CancellationToken cancellationToken)
        {
            var listPropertyTrace = await _propertyTraceObjectRepository.GetByPropertyIdAsync(request.IdProperty);
         
            var propertiesModel = _autoMapper.Map<IEnumerable<PropertyTraceModelOut>>(listPropertyTrace);

            if (propertiesModel is null || !propertiesModel.Any())
            {
                return new TResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "The properties was not Found.",
                    Data = propertiesModel
                };
            }
            return new TResponse() { Data = propertiesModel };

        }
    }
}
