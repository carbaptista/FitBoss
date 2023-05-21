using FitBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitBoss.Application;
public interface IApplicationDbContext
{
    DbSet<Member> Members { get; set; }
    DbSet<Employee> Employees { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
