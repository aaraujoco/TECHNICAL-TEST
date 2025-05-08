using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Interfaces.Services;
using PropertyManager.Application.Common.Mappings;
using PropertyManager.Application.Common.Models;

namespace PropertyManager.Application.Property.Queries;

    public sealed record GetPropertiesPaginatedQuery : IRequest<PaginatedList<PropertyModelOut>>
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public decimal Price { get; set; }

        public string? CodeInternal { get; set; }
        public int Year { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }

public sealed class GetPropertiesPaginatedQueryHandler : IRequestHandler<GetPropertiesPaginatedQuery, PaginatedList<PropertyModelOut>>
{
    private readonly ILogger<GetPropertiesPaginatedQueryHandler> _logger;
    private readonly IPropertyObjectRepository _propertyObjectRepository;
    private readonly IPropertyTraceObjectService _propertyTraceObjectService;
    private readonly IOwnerObjectRepository _ownerObjectRepository;
    private readonly IMapper _autoMapper;

    public GetPropertiesPaginatedQueryHandler(
        IPropertyObjectRepository propertyObjectRepository,
        IPropertyTraceObjectService propertyTraceObjectService,
        IOwnerObjectRepository ownerObjectRepository,
        ILogger<GetPropertiesPaginatedQueryHandler> logger,
        IMapper autoMapper)
    {
        _propertyObjectRepository = propertyObjectRepository;
        _propertyTraceObjectService = propertyTraceObjectService;
        _ownerObjectRepository = ownerObjectRepository;
        _logger = logger;
        _autoMapper = autoMapper;
    }

    public async Task<PaginatedList<PropertyModelOut>> Handle(GetPropertiesPaginatedQuery request, CancellationToken cancellationToken)
    {        

        var totalProperties = await _propertyObjectRepository.GetFilteredCountPropertiesAsync(request);

        var properties = await _propertyObjectRepository.GetFilteredPropertiesAsync(request);

        var propertiesModel = _autoMapper.Map<List<PropertyModelOut>>(properties);

        foreach (var i in propertiesModel)
        {
            var ownerTransaction = await _ownerObjectRepository.GetOwnerByIdAsync(i.Owner!.IdOwner);
            if (ownerTransaction is not null)
            {
                var ownerOut = new OwnerModelOut
                {
                    Address = ownerTransaction.Address,
                    Birthday = ownerTransaction.Birthday,
                    IdOwner = ownerTransaction.IdOwner,
                    Name = ownerTransaction.Name,
                    Photo = ownerTransaction.Photo

                };

                i.Owner = ownerOut;
            }
     
        };

        var tasks = propertiesModel.Select(async i =>
        {
            var listpropertyTraces = await _propertyTraceObjectService.GetByPropertyTraceByIdAsync(i.IdProperty);
            i.PropertyTraces = listpropertyTraces.propertyTraces;
            return i;
        }).ToList();
        

        var propertiesItems = await Task.WhenAll(tasks);

        return await propertiesItems.AsQueryable().PaginatedListAsync(request.Page, request.Size, totalProperties);
        
    }
}

