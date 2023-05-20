namespace FitBoss.Domain.Entities;
public class Employee : Person
{
    public DateOnly HiredDate { get; set; }
    public decimal BaseSalary { get; set; } = 2000;
    public string Branch { get; set; } = "";
}
