namespace HRMS.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public decimal BaseSalary { get; set; }
    public EmploymentStatus Status { get; set; }
    public DateOnly? TerminationDate { get; set; }
    public TerminationType? TerminationType { get; set; }
    public string? TerminationReason { get; set; }

    // Relationships

    // Department
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // LeaveRequest
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new HashSet<LeaveRequest>();
    public ICollection<LeaveRequest> ReviewedLeaveRequests { get; set; } = new HashSet<LeaveRequest>();

    // Self-relationship
    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public ICollection<Employee> Subordinates { get; set; }
        = new HashSet<Employee>();

    // LeaveBalance
    public ICollection<LeaveBalance> LeaveBalances { get; set; }
        = new HashSet<LeaveBalance>();

    // JobTitle
    public int JobTitleId { get; set; }
    public JobTitle JobTitle { get; set; } = null!;

    // AttendanceRecord
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        = new HashSet<AttendanceRecord>();

    public ICollection<Payroll> Payrolls { get; set; } = new HashSet<Payroll>();

    public void Hire()
    {
        Status = EmploymentStatus.Active;
    }

    public bool Terminate(TerminationType terminationType, string reason, DateOnly terminationDate)
    {
        if (Status == EmploymentStatus.Terminated)
            return false;

        Status = EmploymentStatus.Terminated;
        TerminationType = terminationType;
        TerminationReason = reason.Trim();
        TerminationDate = terminationDate;
        return true;
    }

    public void UpdateManager(int? managerId, bool clearManager)
    {
        if (clearManager)
        {
            ManagerId = null;
            Manager = null;
        }
        else if (managerId.HasValue)
        {
            ManagerId = managerId;
            Manager = null;
        }
    }
}
