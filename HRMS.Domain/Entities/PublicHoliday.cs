namespace HRMS.Domain.Entities;

public class PublicHoliday
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly Date { get; set; }

    public void Update(string name, DateOnly date)
    {
        Name = name.Trim();
        Date = date;
    }
    
    public static PublicHoliday Create(string name, DateOnly date)
    {
        return new PublicHoliday
        {
            Name = name,
            Date = date
        };
    }
}
