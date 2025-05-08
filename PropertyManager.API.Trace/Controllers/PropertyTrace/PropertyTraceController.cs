using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyManager.API.Trace.Controllers.Common;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Property.Commands.CreatePropertyTraceObject;
using PropertyManager.Application.Property.Queries;
using PropertyManager.Domain.Common;

namespace PropertyManager.API.Trace.Controllers.PropertyTrace
{
    [Route("api/[controller]")]
    public class PropertyTraceController : ApiControllerBase
    {
        private readonly IValidator<PropertyTraceModel> _validator;
        private readonly IValidator<GetPropertyTraceByPropertyIdQuery> _validatorGetProperty;

        public PropertyTraceController(
        IMediator mediator,
        IValidator<GetPropertyTraceByPropertyIdQuery> validatorGetProperty,
        IValidator<PropertyTraceModel> validator) : base(mediator)
        {
            
            _validatorGetProperty = validatorGetProperty;
            _validator = validator;
        }

        /// <summary>
        /// Get property Traces with by IdProperty. 
        /// </summary>
        /// <returns></returns>
        [HttpPost("find_by")]
        public async Task<IActionResult> FindPropertiesTrace([FromBody] GetPropertyTraceByPropertyIdQuery query, CancellationToken cancellationToken)
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
           
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePropertyTrace([FromBody] CreatePropertyTraceObjectCommand command)
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
                return CreatedAtAction(nameof(CreatePropertyTrace), createProperty);

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

        private async Task<TResponse> ValidatePropertyObject(CreatePropertyTraceObjectCommand request)
        {
            var result = await _validator.ValidateAsync(request.propertyTrace!);
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
}
