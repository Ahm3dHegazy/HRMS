using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HRMS.Api.ResponseHandling;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T? data, string message = "Request completed successfully")
    {
        return new ApiResponse<T>(true, message, data, null);
    }
    
    public static ApiResponse<T> Failure<T>(string message, IReadOnlyDictionary<string, string[]>? errors = null)
    {
        return new ApiResponse<T>(false, message, default, errors);
    }

    public static ApiResponse<object> ValidationFailure(IEnumerable<ValidationFailure> failures,
        string message = "Validation failed")
    {
        var errors = failures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).Distinct().ToArray());

        return Failure<object>(message, errors);
    }

    public static ApiResponse<object> ModelStateFailure(ModelStateDictionary modelState)
    {
        var errors = modelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(entry => entry.Key, entry => entry.Value!.Errors
                .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                    ? "The supplied value is invalid"
                    : error.ErrorMessage)
                .ToArray());

        return Failure<object>("Request validation failed", errors);
    }
}