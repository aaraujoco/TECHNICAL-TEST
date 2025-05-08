using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyManager.API.Controllers.Common;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Property.Commands.CreateOwnerObject;
using PropertyManager.Domain.Common;
using PropertyManager.Domain.Entities;
using System.Net;

namespace PropertyManager.API.Controllers.Property;

[Route("api/[controller]")]
public class OwnerController : ApiControllerBase
{
    private readonly IValidator<OwnerModel> _validator;

    public OwnerController(
        IMediator mediator, 
        IValidator<OwnerModel> validator) : base(mediator)
    {
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOwner([FromBody] CreateOwnerObjectCommand command)
    {
        try
        { 
            var validationResult = await ValidateOwnerObject(command);
            if (validationResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(validationResult);

            var createOwner = await Send(command);
       

            if (createOwner.Errors.Any())
            {
                if (createOwner.StatusCode == HttpStatusCode.NotFound)
                    return NotFound(createOwner);
                else
                    if (createOwner.StatusCode == HttpStatusCode.InternalServerError)
                    return StatusCode(StatusCodes.Status500InternalServerError, createOwner);

                return BadRequest(createOwner);
            }
            return CreatedAtAction(nameof(createOwner), createOwner);

        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new TResponse()
            {
                Message = e.Message,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
    }

    private async Task<TResponse> ValidateOwnerObject(CreateOwnerObjectCommand request)
    {
        var result = await _validator.ValidateAsync(request.Owner!);
        var response = new TResponse();
        var existErrors = result.Errors.Exists(x => x.Severity == Severity.Error);    

        if (!result.IsValid && existErrors)
        {
            response.Message = "Errors have occurred.";
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Errors = result.Errors.Where(x => x.Severity == Severity.Error).Select(e => new Error()
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage
            });
        }
        return response;
    }
}

