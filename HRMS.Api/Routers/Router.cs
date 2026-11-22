namespace HRMS.Api.Routers;

public static class Router
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string ApiVersionPrefix = $"{Root}/{Version}";

    public static class DepartmentRoutes
    {
        private const string DepartmentsPrefix = $"{ApiVersionPrefix}/departments";
        public const string GetAllDepartments = DepartmentsPrefix;
        public const string GetPagedDepartments = $"{DepartmentsPrefix}/paged";
        public const string GetDepartmentById = $"{DepartmentsPrefix}/{{id:int}}";
        public const string CreateDepartment = DepartmentsPrefix;
        public const string UpdateDepartment = $"{DepartmentsPrefix}/{{id:int}}";
        public const string PatchDepartment = $"{DepartmentsPrefix}/{{id:int}}";
        public const string ActivateDepartment = $"{DepartmentsPrefix}/{{id:int}}/activate";
        public const string DeactivateDepartment = $"{DepartmentsPrefix}/{{id:int}}/deactivate";
    }

    public static class AuthenticationRoutes
    {
        private const string AuthenticationPrefix = $"{ApiVersionPrefix}/auth";
        public const string RegisterUser = $"{AuthenticationPrefix}/register";
        public const string LoginUser = $"{AuthenticationPrefix}/login";
        public const string ConfirmEmail = $"{AuthenticationPrefix}/confirm-email";
        public const string LogoutUser = $"{AuthenticationPrefix}/logout";
    }

    public static class EmployeeRoutes
    {
        private const string EmployeesPrefix = $"{ApiVersionPrefix}/employees";

        public const string GetPagedEmployees = $"{EmployeesPrefix}/paged";
        public const string GetEmployeeById = $"{EmployeesPrefix}/{{id:int}}";
        public const string CreateEmployee = EmployeesPrefix;
        public const string UpdateEmployee = $"{EmployeesPrefix}/{{id:int}}";
        public const string PatchEmployee = $"{EmployeesPrefix}/{{id:int}}";
        public const string TerminateEmployee = $"{EmployeesPrefix}/{{id:int}}/terminate";
        public const string DeleteEmployee = $"{EmployeesPrefix}/{{id:int}}";
    }

    public static class MeRoutes
    {
        private const string MePrefix = $"{ApiVersionPrefix}/me";
        public const string GetProfile = $"{MePrefix}/profile";
        public const string GetAttendance = $"{MePrefix}/attendance";
        public const string GetMonthlyAttendance = $"{MePrefix}/attendance/monthly";
        public const string GetLeaveRequests = $"{MePrefix}/leave-requests";
        public const string GetLeaveBalances = $"{MePrefix}/leave-balances";
        public const string GetPayrollHistory = $"{MePrefix}/payroll";
    }

    public static class JobTitleRoutes
    {
        private const string JobTitlesPrefix = $"{ApiVersionPrefix}/job-titles";
        public const string GetAllJobTitles = JobTitlesPrefix;
        public const string GetJobTitleById = $"{JobTitlesPrefix}/{{id:int}}";
        public const string GetEmployeesByJobTitle = $"{JobTitlesPrefix}/{{id:int}}/employees";
        public const string CreateJobTitle = JobTitlesPrefix;
        public const string UpdateJobTitle = $"{JobTitlesPrefix}/{{id:int}}";
        public const string DeleteJobTitle = $"{JobTitlesPrefix}/{{id:int}}";
    }

    public static class LeaveRequestRoutes
    {
        private const string LeaveRequestsPrefix = $"{ApiVersionPrefix}/leave-requests";
        public const string GetPendingLeaveRequests = LeaveRequestsPrefix;
        public const string GetApprovedLeaveRequests = $"{LeaveRequestsPrefix}/approved";
        public const string GetRejectedLeaveRequests = $"{LeaveRequestsPrefix}/rejected";
        public const string GetEmployeeLeaveRequests = $"{LeaveRequestsPrefix}/employee/{{employeeId:int}}";
        public const string GetLeaveRequestById = $"{LeaveRequestsPrefix}/{{id:int}}";
        public const string CreateLeaveRequest = LeaveRequestsPrefix;
        public const string ApproveLeaveRequest = $"{LeaveRequestsPrefix}/{{id:int}}/approve";
        public const string RejectLeaveRequest = $"{LeaveRequestsPrefix}/{{id:int}}/reject";
        public const string CancelLeaveRequest = $"{LeaveRequestsPrefix}/{{id:int}}/cancel";
    }

    public static class PublicHolidayRoutes
    {
        private const string PublicHolidaysPrefix = $"{ApiVersionPrefix}/public-holidays";
        public const string GetAllPublicHolidays = PublicHolidaysPrefix;
        public const string GetPublicHolidayById = $"{PublicHolidaysPrefix}/{{id:int}}";
        public const string CreatePublicHoliday = PublicHolidaysPrefix;
        public const string UpdatePublicHoliday = $"{PublicHolidaysPrefix}/{{id:int}}";
        public const string DeletePublicHoliday = $"{PublicHolidaysPrefix}/{{id:int}}";
    }

    public static class AttendanceRecordRoutes
    {
        private const string AttendanceRecordsPrefix = $"{ApiVersionPrefix}/attendance-records";
        public const string GetAllAttendanceRecords = AttendanceRecordsPrefix;
        public const string GetAttendanceRecordById = $"{AttendanceRecordsPrefix}/{{id:int}}";
        public const string GetEmployeeAttendanceRecords = $"{AttendanceRecordsPrefix}/employee/{{employeeId:int}}";
        public const string GetMonthlyEmployeeAttendanceRecords = $"{AttendanceRecordsPrefix}/employee/{{employeeId:int}}/monthly";
        public const string CreateAttendanceRecord = AttendanceRecordsPrefix;
        public const string CheckInAttendance = $"{AttendanceRecordsPrefix}/check-in";
        public const string CheckOutAttendance = $"{AttendanceRecordsPrefix}/check-out";
        public const string UpdateAttendanceRecord = $"{AttendanceRecordsPrefix}/{{id:int}}";
        public const string DeleteAttendanceRecord = $"{AttendanceRecordsPrefix}/{{id:int}}";
    }

    public static class AdminDashboardRoutes
    {
        private const string AdminDashboardPrefix = $"{ApiVersionPrefix}/admin/dashboard";
        public const string GetOverview = $"{AdminDashboardPrefix}/overview";
        public const string GetAttendance = $"{AdminDashboardPrefix}/attendance";
        public const string GetAttendanceTrend = $"{AdminDashboardPrefix}/attendance/trend";
        public const string GetPendingLeaves = $"{AdminDashboardPrefix}/pending-leaves";
        public const string GetLeaveSummary = $"{AdminDashboardPrefix}/leave/summary";
        public const string GetPayrollSummary = $"{AdminDashboardPrefix}/payroll/summary";
        public const string GetRecentActivities = $"{AdminDashboardPrefix}/recent-activities";
    }

    public static class PayrollRoutes
    {
        private const string PayrollPrefix = $"{ApiVersionPrefix}/payroll";
        public const string GeneratePayroll = $"{PayrollPrefix}/generate";
        public const string GenerateMonthlyPayroll = $"{PayrollPrefix}/generate-monthly";
        public const string GetPayrollById = $"{PayrollPrefix}/{{id:int}}";
        public const string GetEmployeePayrollHistory = $"{PayrollPrefix}/employee/{{employeeId:int}}";
        public const string MarkPayrollAsPaid = $"{PayrollPrefix}/{{id:int}}/paid";
    }
}
