using FitBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitBoss.Application;
public interface IApplicationDbContext
{
    DbSet<Trainer> Members { get; set; }
    DbSet<Trainer> Trainers { get; set; }
    DbSet<Manager> Managers { get; set; }
    DbSet<Receptionist> Receptionists { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
