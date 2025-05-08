using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyManager.API.Controllers.Common;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Property.Commands.CreatePropertyObject;
using PropertyManager.Application.Property.Commands.UpdatePropertyObject;
using PropertyManager.Application.Property.Commands.UploadPropertyImageObject;
using PropertyManager.Application.Property.Queries;
using PropertyManager.Domain.Common;
using System.Net;

namespace PropertyManager.API.Controllers.Property;

[Route("api/[controller]")]
public class PropertyController : ApiControllerBase
{
    private readonly IValidator<PropertyModel> _validator;
    private readonly IValidator<PropertyPriceModel> _validatorPrice;
    private readonly IValidator<PropertyUpdateModel> _validatorUpdate;
    private readonly IValidator<UploadPropertyImageModel> _validatorUploadImage;
    private readonly IValidator<GetPropertiesPaginatedQuery> _validatorGetProperty;

    public PropertyController(
        IMediator mediator,
        IValidator<PropertyModel> validator,
        IValidator<PropertyPriceModel> validatorPrice,
        IValidator<PropertyUpdateModel> validatorUpdate,
        IValidator<UploadPropertyImageModel> validatorUploadImage,
        IValidator<GetPropertiesPaginatedQuery> validatorGetProperty) : base(mediator)
    {
        _validator = validator;
        _validatorPrice = validatorPrice;
        _validatorUpdate = validatorUpdate;
        _validatorUploadImage = validatorUploadImage;
        _validatorGetProperty = validatorGetProperty;
    }

    /// <summary>
    /// Get propery Paginated with filter. 
    /// </summary>
    /// <returns></returns>
    [HttpPost("find")]
    public async Task<IActionResult> FindProperties([FromBody] GetPropertiesPaginatedQuery query, CancellationToken cancellationToken)
    {
        var result = await _validatorGetProperty.ValidateAsync(query);
        var existErrors = result.Errors.Exists(x => x.Severity == Severity.Error);

        if (!result.IsValid && existErrors)
        {
            return BadRequest(new TResponse()
            {
                Message = "Error have occur.",
                Errors = result.Errors.Select(e => new Error
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                })
            });
        }


        var response = await Send(query);

        return Ok(new TResponse()
        {
            Message = $"{response.TotalCount} properties were found.",
            Data = response,
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyObjectCommand command)
    {
        try
        {
            var validationResult = await ValidatePropertyObject(command);
            if (validationResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(validationResult);

            var createProperty = await Send(command);

            if (createProperty.Errors.Any())
            {
                if (createProperty.StatusCode == HttpStatusCode.NotFound)
                    return NotFound(createProperty);
                else
                    if (createProperty.StatusCode == HttpStatusCode.InternalServerError)
                    return StatusCode(StatusCodes.Status500InternalServerError, createProperty);

                return BadRequest(createProperty);
            }
            return CreatedAtAction(nameof(createProperty), createProperty);

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

    /// <summary>
    /// Update Property.
    /// </summary>
    /// <param name="command">Receive from query the PropertyId and NewPrice to update</param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> UpdateProperty([FromBody] UpdatePropertyObjectCommand command)
    {
        try
        {
            var validationResult = await ValidatePropertyUpdateObject(command);
            if (validationResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(validationResult);

            var updateProperty = await Send(command);

            if (updateProperty.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateProperty);

            if (updateProperty.Errors.Any())
            {
                if (updateProperty.StatusCode == HttpStatusCode.InternalServerError)
                    return StatusCode(StatusCodes.Status500InternalServerError, updateProperty);

                return BadRequest(updateProperty);
            }
            return Ok(updateProperty);

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

    /// <summary>
    /// Update Property.
    /// </summary>
    /// <param name="command">Receive from query the PropertyId and NewPrice to update</param>
    /// <returns></returns>
    [HttpPut("price")]
    public async Task<IActionResult> UpdateNewPriceProperty([FromBody] UpdatePropertyPriceObjectCommand command)
    {
        try
        {
            var validationResult = await ValidatePropertyPriceObject(command);
            if (validationResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(validationResult);

            var updateProperty = await Send(command);

            if (updateProperty.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateProperty);

            if (updateProperty.Errors.Any())
            {
                if (updateProperty.StatusCode == HttpStatusCode.InternalServerError)
                    return StatusCode(StatusCodes.Status500InternalServerError, updateProperty);

                return BadRequest(updateProperty);
            }
            return Ok(updateProperty);

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

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] UploadPropertyImageObjectCommand command)
    {
        try
        {
            var validationResult = await ValidatePropertyImageObject(command);
            if (validationResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(validationResult);

            var updateProperty = await Send(command);

            if (updateProperty.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateProperty);

            if (updateProperty.Errors.Any())
            {
                if (updateProperty.StatusCode == HttpStatusCode.InternalServerError)
                    return StatusCode(StatusCodes.Status500InternalServerError, updateProperty);

                return BadRequest(updateProperty);
            }
            return CreatedAtAction(nameof(UploadImage), updateProperty);
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
    private async Task<TResponse> ValidatePropertyObject(CreatePropertyObjectCommand request)
    {
        var result = await _validator.ValidateAsync(request.property!);
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

      private async Task<TResponse> ValidatePropertyUpdateObject(UpdatePropertyObjectCommand request)
    {
        var result = await _validatorUpdate.ValidateAsync(request.property!);
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

    private async Task<TResponse> ValidatePropertyPriceObject(UpdatePropertyPriceObjectCommand request)
    {
        var result = await _validatorPrice.ValidateAsync(request.propertyPrice!);
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

    private async Task<TResponse> ValidatePropertyImageObject(UploadPropertyImageObjectCommand request)
    {
        var result = await _validatorUploadImage.ValidateAsync(request.propertyImage!);
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

