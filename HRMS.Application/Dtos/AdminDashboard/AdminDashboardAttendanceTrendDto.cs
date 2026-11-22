namespace HRMS.Application.Dtos.AdminDashboard;

public sealed record AdminDashboardAttendanceTrendDto(
    DateOnly From,
    DateOnly To,
    IReadOnlyList<AdminDashboardAttendanceTrendPointDto> Days);

public sealed record AdminDashboardAttendanceTrendPointDto(
    DateOnly Date,
    int Present,
    int Late,
    int Absent,
    int OnLeave);
