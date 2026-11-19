namespace HRMS.Application.Dtos.Employee;

public class TerminatedEmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public decimal BaseSalary { get; set; }
    public string TerminationType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateOnly TerminationDate { get; set; }
}
