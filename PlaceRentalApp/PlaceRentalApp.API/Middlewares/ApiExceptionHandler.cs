using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.Application.Exceptions;

namespace PlaceRentalApp.API.Middlewares
{
    public class ApiExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails? details;

            if (exception is NotFoundException) 
            {
                details = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = exception.Message
                };
            }
            else
            {
                details = new ProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                };
            }
            // Log se preferir

            httpContext.Response.StatusCode = details.Status ?? StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

            return true;
        }
    }
}
