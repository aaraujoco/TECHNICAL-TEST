using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Common;
using PropertyManager.Domain.Entities;
using System.Net;

namespace PropertyManager.Application.Property.Commands.CreateOwnerObject;
public class CreateOwnerObjectCommand : IRequest<TResponse>
{
    public OwnerModel? Owner { get; set; }
}
public class CreateOwnerObjectCommandHandler : IRequestHandler<CreateOwnerObjectCommand, TResponse>
{
    private readonly ILogger<CreateOwnerObjectCommandHandler> _logger;
    private readonly IOwnerObjectRepository _ownerObjectRepository;
    private readonly IMapper _autoMapper;
    public CreateOwnerObjectCommandHandler(
        IOwnerObjectRepository ownerObjectRepository,
        ILogger<CreateOwnerObjectCommandHandler> logger,
        IMapper autoMapper)
    {
        _ownerObjectRepository = ownerObjectRepository;
        _logger = logger;
        _autoMapper = autoMapper;
    }
    public async Task<TResponse> Handle(CreateOwnerObjectCommand request, CancellationToken cancellationToken)
    {
        var statusCode = HttpStatusCode.OK;

        var message = "The process was carried out correctly.";

        var result = "NoChange";

        try
        {
            var owner = _autoMapper.Map<Owner>(request.Owner);
            owner.CreatedDate = DateTime.Now;
            owner.UpdatedDate = DateTime.Now;
            await _ownerObjectRepository.AddOwnerAsync(owner);
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
        message = "The owner has been created successfully.";
        result = "Created";

        return new TResponse()
        {
            Result = result,
            Message = message,
            StatusCode = statusCode,
            Data = request.Owner
        };
    }
}

