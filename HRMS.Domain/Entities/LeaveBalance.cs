namespace HRMS.Domain.Entities;

public class LeaveBalance
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int TotalDays { get; set; }
    public int UsedDays { get; set; }

    // Relationships

    // Employee
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // LeaveType
    public int LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = null!;

    public int AvailableDays => TotalDays - UsedDays;

    public bool CanDeduct(int days) => days > 0 && AvailableDays >= days;

    public bool TryDeduct(int days)
    {
        if (!CanDeduct(days))
            return false;

        UsedDays += days;
        return true;
    }
}
