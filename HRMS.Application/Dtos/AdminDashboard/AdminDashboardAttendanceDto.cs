namespace HRMS.Application.Dtos.AdminDashboard;

public sealed record AdminDashboardAttendanceDto(
    DateOnly Date,
    int TotalRecords,
    int Present,
    int Late,
    int Absent,
    int OnLeave);
