namespace HRMS.Application.Common.Models;

public record EmployeeFilter(string? Search = null, int? DepartmentId = null, EmploymentStatus? Status = null);
