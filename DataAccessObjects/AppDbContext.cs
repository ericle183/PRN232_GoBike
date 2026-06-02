using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace DataAccessObjects;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<MotorcycleType> MotorcycleTypes => Set<MotorcycleType>();
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<RentalContract> RentalContracts => Set<RentalContract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique indexes
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Motorcycle>().HasIndex(m => m.LicensePlate).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.CCCD).IsUnique();

        // Decimal precision
        modelBuilder.Entity<MotorcycleType>().Property(m => m.DefaultDailyRate).HasPrecision(18, 2);
        modelBuilder.Entity<Motorcycle>().Property(m => m.DailyRate).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.DailyRate).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.DepositAmount).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.FinalAmount).HasPrecision(18, 2);

        // Soft delete query filters
        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
        modelBuilder.Entity<MotorcycleType>().HasQueryFilter(m => m.IsActive);
        modelBuilder.Entity<Motorcycle>().HasQueryFilter(m => m.IsActive);
        modelBuilder.Entity<Customer>().HasQueryFilter(c => c.IsActive);
        modelBuilder.Entity<RentalContract>().HasQueryFilter(r => r.IsActive);
    }
}
