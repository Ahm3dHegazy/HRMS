namespace HRMS.Application.Authentication;

public interface ICurrentUser
{
    int? EmployeeId { get; }
}
