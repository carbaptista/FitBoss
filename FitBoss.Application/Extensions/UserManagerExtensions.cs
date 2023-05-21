using FitBoss.Application;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Extensions;
public static class UserManagerExtensions
{
    public static async Task<Employee> CreateEmployee(
        this UserManager<BaseEntity> userManager,
        Employee employee,
        IApplicationDbContext _context)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }
}
