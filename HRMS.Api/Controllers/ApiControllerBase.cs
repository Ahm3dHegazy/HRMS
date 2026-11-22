namespace HRMS.Api.Controllers;

[Authorize]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Converts an application result into the matching HTTP response and API response envelope.
    /// </summary>
    protected IActionResult HandleResult<T>(Result<T> result, 
        string? successMessage = null,
        int successStatusCode = StatusCodes.Status200OK)
    {
        if (result.IsFailure)
        {
            var error = result.Error!;
            return HandleFailure(error.Code, error.Message);
        }

        return StatusCode(successStatusCode, ApiResponseFactory.Success(result.Value,
            successMessage ?? "Request completed successfully"));
    }

    private IActionResult HandleFailure(string errorCode, string errorMessage)
    {
        var response = ApiResponseFactory.Failure<object>(errorMessage);

        return errorCode switch
        {
            "NotFound" => NotFound(response),
            "Conflict" => Conflict(response),
            "Unauthorized" => Unauthorized(response),
            "Forbidden" => StatusCode(StatusCodes.Status403Forbidden, response),
            _ => BadRequest(response)
        };
    }
}
