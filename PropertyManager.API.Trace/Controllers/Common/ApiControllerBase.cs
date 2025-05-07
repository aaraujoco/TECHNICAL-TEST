using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PropertyManager.API.Trace.Controllers.Common
{

    /// <summary>
    /// ApiControllerBase is an abstract base class for API controllers that utilizes the Mediator pattern to handle requests and notifications.
    /// </summary>
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
      
            private readonly IMediator _mediator;
            /// <summary>
            /// Initializes a new instance of the <see cref = "ApiControllerBase"/> class.
            /// </summary>
            /// <param name = "mediator">An instance of IMediator interface which is used to send requests and notifications.</param>
            protected ApiControllerBase(IMediator mediator) => _mediator = mediator;
            /// <summary>
            /// Send a request and get the response.
            /// </summary>
            /// <param name = "request">The request that needs to be handled.</param>
            /// <typeparam name = "TResponse">The type of response expected.</typeparam>
            /// <returns>A Task that represents the asynchronous operation, containing the response as a result.</returns>
            protected Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
            {
                return _mediator.Send(request);
            }

            /// <summary>
            /// Send a request without expecting a response.
            /// </summary>
            /// <param name = "request">The request that needs to be handled.</param>
            /// <returns>A Task that represents the asynchronous operation.</returns>
            protected Task Send(IRequest request)
            {
                return _mediator.Send(request);
            }

            /// <summary>
            /// Publish a notification.
            /// </summary>
            /// <param name = "notification">The notification that needs to be handled.</param>
            /// <typeparam name = "TNotification">The type of notification.</typeparam>
            /// <returns>A Task that represents the asynchronous operation.</returns>
            protected Task Publish<TNotification>(TNotification notification)
                where TNotification : INotification
            {
                return _mediator.Publish(notification);
            }
        }
}
