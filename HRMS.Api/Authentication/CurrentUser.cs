namespace HRMS.Api.Authentication;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public int? EmployeeId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User
                .FindFirst(AppClaimTypes.EmployeeId)?.Value;

            return int.TryParse(value, out var employeeId) ? employeeId : null;
        }
    }
}
