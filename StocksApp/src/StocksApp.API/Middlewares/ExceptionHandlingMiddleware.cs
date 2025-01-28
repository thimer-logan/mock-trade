using StocksApp.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace StocksApp.API.Middlewares
{
    /// <summary>
    /// A custom middleware that catches unhandled exceptions and 
    /// translates them into appropriate HTTP responses.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // An unhandled exception occurred, handle it here
                _logger.LogError(ex, "An unhandled exception has occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Default to 500 Internal Server Error
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = notFoundEx.Message;
                    break;

                case UnauthorizedResourceAccessException uaEx:
                    statusCode = StatusCodes.Status403Forbidden;
                    message = uaEx.Message;
                    break;

                case InsufficientSharesException isEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = isEx.Message;
                    break;

                default:

                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
