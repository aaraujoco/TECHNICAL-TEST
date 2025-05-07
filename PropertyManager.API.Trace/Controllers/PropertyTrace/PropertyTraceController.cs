using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyManager.API.Trace.Controllers.Common;
using PropertyManager.Application.Property.Queries;
using PropertyManager.Domain.Common;

namespace PropertyManager.API.Trace.Controllers.PropertyTrace
{
    [Route("api/[controller]")]
    public class PropertyTraceController : ApiControllerBase
    {
        private readonly IValidator<GetPropertyTraceByPropertyIdQuery> _validatorGetProperty;

        public PropertyTraceController(
        IMediator mediator,
        IValidator<GetPropertyTraceByPropertyIdQuery> validatorGetProperty) : base(mediator)
        {
            
            _validatorGetProperty = validatorGetProperty;
        }

        /// <summary>
        /// Get propery Paginated with filter. 
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

    }
}
