using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessObjects;

public static class DbSeed
{
    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<MotorcycleType>().HasData(
            new MotorcycleType { Id = 1, Name = "Xe số", Description = "Motorcycle with manual transmission", DefaultDailyRate = 100000, IsActive = true },
            new MotorcycleType { Id = 2, Name = "Xe tay ga", Description = "Scooter with automatic transmission", DefaultDailyRate = 150000, IsActive = true },
            new MotorcycleType { Id = 3, Name = "Xe phan khối lớn", Description = "High displacement motorcycle", DefaultDailyRate = 250000, IsActive = true }
        );

        builder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", PasswordHash = "$2a$11$rDkPvvAFV8LQ8jRj1k1K5eQWjN1p1Qq6kXqK8mR5sT3uV4wW5xX6y", FullName = "Admin User", Email = "admin@gobike.vn", Role = UserRole.Admin, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 2, Username = "staff", PasswordHash = "$2a$11$sLkQvvAFV8LQ8jRj1k1K5eQWjN1p1Qq6kXqK8mR5sT3uV4wW5xX6y", FullName = "Staff User", Email = "staff@gobike.vn", Role = UserRole.Staff, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        builder.Entity<Motorcycle>().HasData(
            new Motorcycle { Id = 1, LicensePlate = "51A-12345", Brand = "Honda", Model = "Wave RSX", VehicleTypeId = 1, Status = MotorcycleStatus.Available, DailyRate = 120000, Color = "Black", Mileage = 5000, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 2, LicensePlate = "51B-54321", Brand = "Yamaha", Model = "Exciter", VehicleTypeId = 1, Status = MotorcycleStatus.Available, DailyRate = 130000, Color = "Red", Mileage = 8000, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 3, LicensePlate = "29A-11111", Brand = "Honda", Model = "Vision", VehicleTypeId = 2, Status = MotorcycleStatus.Available, DailyRate = 150000, Color = "White", Mileage = 3000, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 4, LicensePlate = "30B-22222", Brand = "Piaggio", Model = "Liberty", VehicleTypeId = 2, Status = MotorcycleStatus.Available, DailyRate = 160000, Color = "Gray", Mileage = 4500, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 5, LicensePlate = "48C-33333", Brand = "BMW", Model = "GS310R", VehicleTypeId = 3, Status = MotorcycleStatus.Available, DailyRate = 350000, Color = "Blue", Mileage = 2000, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 6, LicensePlate = "47D-44444", Brand = "Honda", Model = "Lead", VehicleTypeId = 2, Status = MotorcycleStatus.Maintenance, DailyRate = 140000, Color = "Silver", Mileage = 12000, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 7, LicensePlate = "43E-55555", Brand = "Suzuki", Model = "Address", VehicleTypeId = 2, Status = MotorcycleStatus.Available, DailyRate = 130000, Color = "Black", Mileage = 6000, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 8, LicensePlate = "16F-66666", Brand = "Yamaha", Model = "MT-03", VehicleTypeId = 3, Status = MotorcycleStatus.Available, DailyRate = 280000, Color = "Black", Mileage = 1500, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 9, LicensePlate = "60G-77777", Brand = "Honda", Model = "Future", VehicleTypeId = 1, Status = MotorcycleStatus.Available, DailyRate = 110000, Color = "Blue", Mileage = 9000, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Motorcycle { Id = 10, LicensePlate = "59H-88888", Brand = "Vespa", Model = "Sprint", VehicleTypeId = 2, Status = MotorcycleStatus.Available, DailyRate = 200000, Color = "Green", Mileage = 2500, IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        builder.Entity<Customer>().HasData(
            new Customer { Id = 1, FullName = "Nguyen Van A", CCCD = "012345678901", PhoneNumber = "0912345678", Email = "nguyenvana@email.com", Address = "123 Nguyen Trai, Q1, HCM", DateOfBirth = new DateTime(1990, 5, 15), DriverLicenseNo = "DL001", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 2, FullName = "Tran Thi B", CCCD = "012345678902", PhoneNumber = "0912345679", Email = "tranthib@email.com", Address = "456 Le Lai, Q3, HCM", DateOfBirth = new DateTime(1992, 8, 20), DriverLicenseNo = "DL002", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 3, FullName = "Le Van C", CCCD = "012345678903", PhoneNumber = "0912345680", Email = "levanc@email.com", Address = "789 Tran Hung Dao, Q5, HCM", DateOfBirth = new DateTime(1988, 3, 10), DriverLicenseNo = "DL003", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 4, FullName = "Pham Thi D", CCCD = "012345678904", PhoneNumber = "0912345681", Email = "phamthid@email.com", Address = "321 Vo Van Tien, Q4, HCM", DateOfBirth = new DateTime(1995, 11, 25), DriverLicenseNo = "DL004", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 5, FullName = "Hoang Van E", CCCD = "012345678905", PhoneNumber = "0912345682", Email = "hoangvane@email.com", Address = "654 Pham Van Dong, Thu Duc, HCM", DateOfBirth = new DateTime(1991, 7, 8), DriverLicenseNo = "DL005", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 6, FullName = "Dinh Thi F", CCCD = "012345678906", PhoneNumber = "0912345683", Email = "dinhthif@email.com", Address = "987 Cong Hoa, Tan Binh, HCM", DateOfBirth = new DateTime(1993, 2, 14), DriverLicenseNo = "DL006", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 7, FullName = "Bui Van G", CCCD = "012345678907", PhoneNumber = "0912345684", Email = "buivang@email.com", Address = "135 Thai Nguyen, Tan Phu, HCM", DateOfBirth = new DateTime(1989, 9, 30), DriverLicenseNo = "DL007", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 8, FullName = "Do Thi H", CCCD = "012345678908", PhoneNumber = "0912345685", Email = "dothih@email.com", Address = "246 Ly Thuong Kiet, Q11, HCM", DateOfBirth = new DateTime(1994, 12, 5), DriverLicenseNo = "DL008", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 9, FullName = "Ngo Van I", CCCD = "012345678909", PhoneNumber = "0912345686", Email = "ngovani@email.com", Address = "864 Au Duong Lan, Q8, HCM", DateOfBirth = new DateTime(1996, 4, 22), DriverLicenseNo = "DL009", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 10, FullName = "Vu Thi K", CCCD = "012345678910", PhoneNumber = "0912345687", Email = "vuthik@email.com", Address = "753 Nguyen Oanh, Go Vap, HCM", DateOfBirth = new DateTime(1997, 6, 18), DriverLicenseNo = "DL010", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        builder.Entity<RentalContract>().HasData(
            new RentalContract { Id = 1, CustomerId = 1, MotorcycleId = 1, RentalDate = new DateTime(2026, 5, 20), ExpectedReturnDate = new DateTime(2026, 5, 25), ActualReturnDate = new DateTime(2026, 5, 24), DailyRate = 120000, TotalAmount = 600000, DepositAmount = 500000, FinalAmount = 600000, Status = RentalStatus.Completed, StartMileage = 5000, EndMileage = 5200, CreatedBy = "admin", IsActive = true, CreatedAt = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc) },
            new RentalContract { Id = 2, CustomerId = 2, MotorcycleId = 3, RentalDate = new DateTime(2026, 5, 25), ExpectedReturnDate = new DateTime(2026, 5, 28), ActualReturnDate = new DateTime(2026, 5, 28), DailyRate = 150000, TotalAmount = 450000, DepositAmount = 300000, FinalAmount = 450000, Status = RentalStatus.Completed, StartMileage = 3000, EndMileage = 3150, CreatedBy = "admin", IsActive = true, CreatedAt = new DateTime(2026, 5, 25, 0, 0, 0, DateTimeKind.Utc) },
            new RentalContract { Id = 3, CustomerId = 3, MotorcycleId = 5, RentalDate = new DateTime(2026, 5, 28), ExpectedReturnDate = new DateTime(2026, 5, 30), ActualReturnDate = new DateTime(2026, 5, 30), DailyRate = 350000, TotalAmount = 700000, DepositAmount = 1000000, FinalAmount = 700000, Status = RentalStatus.Completed, StartMileage = 2000, EndMileage = 2100, CreatedBy = "admin", IsActive = true, CreatedAt = new DateTime(2026, 5, 28, 0, 0, 0, DateTimeKind.Utc) },
            new RentalContract { Id = 4, CustomerId = 4, MotorcycleId = 2, RentalDate = new DateTime(2026, 6, 1), ExpectedReturnDate = new DateTime(2026, 6, 5), DailyRate = 130000, TotalAmount = 520000, DepositAmount = 200000, FinalAmount = 0, Status = RentalStatus.Active, StartMileage = 8000, CreatedBy = "admin", IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RentalContract { Id = 5, CustomerId = 5, MotorcycleId = 7, RentalDate = new DateTime(2026, 5, 30), ExpectedReturnDate = new DateTime(2026, 5, 31), DailyRate = 130000, TotalAmount = 130000, DepositAmount = 100000, FinalAmount = 0, Status = RentalStatus.Pending, CreatedBy = "admin", IsActive = true, CreatedAt = new DateTime(2026, 5, 30, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
