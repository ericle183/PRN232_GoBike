using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessObjects;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<MotorcycleType> MotorcycleTypes { get; set; }
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<RentalContract> RentalContracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Motorcycle>().HasIndex(m => m.LicensePlate).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.CCCD).IsUnique();
        modelBuilder.Entity<MotorcycleType>().HasIndex(m => m.Name).IsUnique();

        modelBuilder.Entity<Motorcycle>().Property(m => m.DailyRate).HasPrecision(18, 2);
        modelBuilder.Entity<MotorcycleType>().Property(m => m.DefaultDailyRate).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.DailyRate).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.DepositAmount).HasPrecision(18, 2);
        modelBuilder.Entity<RentalContract>().Property(r => r.FinalAmount).HasPrecision(18, 2);

        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
        modelBuilder.Entity<Motorcycle>().HasQueryFilter(m => m.IsActive);
        modelBuilder.Entity<Customer>().HasQueryFilter(c => c.IsActive);
        modelBuilder.Entity<RentalContract>().HasQueryFilter(r => r.IsActive);
        modelBuilder.Entity<MotorcycleType>().HasQueryFilter(m => m.IsActive);

        DbSeed.Seed(modelBuilder);
    }
}
