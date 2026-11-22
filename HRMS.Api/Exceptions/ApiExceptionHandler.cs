using Microsoft.AspNetCore.Diagnostics;

namespace HRMS.Api.Exceptions;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            var isConflict = validationException.Errors.Any(error => error.ErrorCode == "Conflict");
            httpContext.Response.StatusCode = isConflict
                ? StatusCodes.Status409Conflict
                : StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(
                ApiResponseFactory.ValidationFailure(
                    validationException.Errors,
                    isConflict
                        ? validationException.Errors.First(error => error.ErrorCode == "Conflict").ErrorMessage
                        : "Validation failed"),
                cancellationToken);

            return true;
        }

        logger.LogError(exception, "Unhandled API exception");
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            ApiResponseFactory.Failure<object>("An unexpected error occurred"),
            cancellationToken);

        return true;
    }
}
