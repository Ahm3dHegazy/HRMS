namespace HRMS.Domain.Entities;

public class Payroll
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal AbsenceDeduction { get; set; }
    public decimal LateDeduction { get; set; }
    public decimal UnpaidLeaveDeduction { get; set; }
    public decimal NetSalary { get; set; }
    public PayrollStatus Status { get; private set; } = PayrollStatus.Unpaid;
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; private set; }

    public bool MarkAsPaid(DateTime paidAt)
    {
        if (Status == PayrollStatus.Paid) return false;
        Status = PayrollStatus.Paid;
        PaidAt = paidAt;
        return true;
    }
}
