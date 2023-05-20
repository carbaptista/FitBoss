using FitBoss.Application;
using FitBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Trainer> Members { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Receptionist> Receptionists { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Trainer>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Manager>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Trainer>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Receptionist>()
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}
