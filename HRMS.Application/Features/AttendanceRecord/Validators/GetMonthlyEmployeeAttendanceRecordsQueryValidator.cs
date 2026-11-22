using HRMS.Application.Features.AttendanceRecord.Queries.GetMonthlyEmployeeAttendanceRecords;

namespace HRMS.Application.Features.AttendanceRecord.Validators;

public sealed class GetMonthlyEmployeeAttendanceRecordsQueryValidator
    : AbstractValidator<GetMonthlyEmployeeAttendanceRecordsQuery>
{
    public GetMonthlyEmployeeAttendanceRecordsQueryValidator()
    {
        RuleFor(query => query.EmployeeId).GreaterThan(0);
        RuleFor(query => query.Year).InclusiveBetween(1, 9998);
        RuleFor(query => query.Month).InclusiveBetween(1, 12);
    }
}

