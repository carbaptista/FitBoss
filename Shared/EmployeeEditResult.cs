using FitBoss.Domain.Entities;

namespace Shared;
public class EmployeeEditResult
{
    public List<string> messages { get; set; } = new();
    public bool succeeded { get; set; }
    public Employee? data { get; set; }
    public Exception? exception { get; set; }
}
