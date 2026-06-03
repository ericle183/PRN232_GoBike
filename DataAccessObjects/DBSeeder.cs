using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace DataAccessObjects;

public static class DBSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var adminHash = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 11);
        var staffHash = BCrypt.Net.BCrypt.HashPassword("staff123", workFactor: 11);

        // --- 1. Users ---
        var users = new List<User>
        {
            new()
            {
                Username = "admin", PasswordHash = adminHash, FullName = "Admin User",
                Email = "admin@gobike.vn",
                Role = BusinessObjects.Enums.UserRole.Admin,
                IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Username = "staff", PasswordHash = staffHash, FullName = "Staff User",
                Email = "staff@gobike.vn",
                Role = BusinessObjects.Enums.UserRole.Staff,
                IsActive = true, CreatedAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc)
            },
        };
        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        // --- 2. MotorcycleTypes ---
        var types = new List<MotorcycleType>
        {
            new() { Name = "Xe số",            Description = "Xe máy số tự động",                          DefaultDailyRate = 100_000m, IsActive = true },
            new() { Name = "Xe tay ga",         Description = "Xe tay ga automatic",                          DefaultDailyRate = 150_000m, IsActive = true },
            new() { Name = "Xe phân khối lớn",Description = "Xe phân khối lớn từ 150cc trở lên",           DefaultDailyRate = 250_000m, IsActive = true },
        };
        await context.MotorcycleTypes.AddRangeAsync(types);
        await context.SaveChangesAsync();

        // --- 3. Motorcycles (10 sample) ---
        var motorcycles = new List<Motorcycle>
        {
            new() { LicensePlate = "51A-12345", Brand = "Honda",     Model = "Wave RSX",  VehicleTypeId = 1, Status = BusinessObjects.Enums.MotorcycleStatus.Available,   DailyRate = 100_000m, Color = "Đen",   Mileage = 12000, RegistrationNo = "REG-001", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "51B-23456", Brand = "Yamaha",    Model = "Exciter",   VehicleTypeId = 1, Status = BusinessObjects.Enums.MotorcycleStatus.Available,   DailyRate = 120_000m, Color = "Đỏ",    Mileage = 8500,  RegistrationNo = "REG-002", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "29C-34567", Brand = "Suzuki",    Model = "Raider",    VehicleTypeId = 1, Status = BusinessObjects.Enums.MotorcycleStatus.Rented,       DailyRate = 110_000m, Color = "Xanh",  Mileage = 15000, RegistrationNo = "REG-003", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "48D-45678", Brand = "Honda",     Model = "Future",    VehicleTypeId = 1, Status = BusinessObjects.Enums.MotorcycleStatus.Available,   DailyRate = 100_000m, Color = "Trắng", Mileage = 5000,  RegistrationNo = "REG-004", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "51E-56789", Brand = "Honda",     Model = "Lead",      VehicleTypeId = 2, Status = BusinessObjects.Enums.MotorcycleStatus.Available,   DailyRate = 150_000m, Color = "Bạc",   Mileage = 7200,  RegistrationNo = "REG-005", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "16F-67890", Brand = "Yamaha",    Model = "Nouvo",     VehicleTypeId = 2, Status = BusinessObjects.Enums.MotorcycleStatus.Maintenance, DailyRate = 140_000m, Color = "Đen",   Mileage = 22000, RegistrationNo = "REG-006", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "30G-78901", Brand = "Piaggio",   Model = "Liberty",   VehicleTypeId = 2, Status = BusinessObjects.Enums.MotorcycleStatus.Available,   DailyRate = 160_000m, Color = "Xám",   Mileage = 4300,  RegistrationNo = "REG-007", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "59H-89012", Brand = "Honda",     Model = "PCX",       VehicleTypeId = 2, Status = BusinessObjects.Enums.MotorcycleStatus.Reserved,    DailyRate = 180_000m, Color = "Đỏ",    Mileage = 3100,  RegistrationNo = "REG-008", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "51K-90123", Brand = "Yamaha",    Model = "MT-03",     VehicleTypeId = 3, Status = BusinessObjects.Enums.MotorcycleStatus.Available,   DailyRate = 300_000m, Color = "Xám",   Mileage = 2500,  RegistrationNo = "REG-009", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { LicensePlate = "47L-01234", Brand = "Kawasaki",  Model = "Z125",      VehicleTypeId = 3, Status = BusinessObjects.Enums.MotorcycleStatus.Available,   DailyRate = 350_000m, Color = "Xanh",  Mileage = 1800,  RegistrationNo = "REG-010", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
        };
        await context.Motorcycles.AddRangeAsync(motorcycles);
        await context.SaveChangesAsync();

        // --- 4. Customers (10 sample) ---
        var customers = new List<Customer>
        {
            new() { FullName = "Nguyễn Văn An",     CCCD = "079206001234", PhoneNumber = "0912345678", Email = "an.nv@gmail.com",       Address = "123 Lê Lợi, Q1, TP.HCM",         DateOfBirth = new DateTime(1995, 3, 15),  DriverLicenseNo = "DL001234", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Trần Thị Bình",    CCCD = "079206005678", PhoneNumber = "0912345679", Email = "binh.tt@gmail.com",      Address = "45 Nguyễn Trãi, Q5, TP.HCM",      DateOfBirth = new DateTime(1990, 7, 22),  DriverLicenseNo = "DL005678", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Lê Hoàng Cường",   CCCD = "079206009012", PhoneNumber = "0934567890", Email = "cuong.lh@gmail.com",     Address = "78 Pasteur, Q1, TP.HCM",           DateOfBirth = new DateTime(1998, 11, 8),  DriverLicenseNo = "DL009012", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Phạm Minh Đức",   CCCD = "079206013456", PhoneNumber = "0945678901", Email = "duc.pm@gmail.com",      Address = "12 Điện Biên Phủ, Bình Thạnh",    DateOfBirth = new DateTime(1988, 5, 30),  DriverLicenseNo = "DL013456", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Hoàng Thu Hà",    CCCD = "079206017890", PhoneNumber = "0956789012", Email = "ha.ht@gmail.com",       Address = "56 Võ Văn Tần, Q3, TP.HCM",       DateOfBirth = new DateTime(1993, 1, 12),  DriverLicenseNo = "DL017890", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Đặng Quốc Khánh", CCCD = "079206021234", PhoneNumber = "0967890123", Email = "khanh.dq@gmail.com",    Address = "90 Lý Thường Kiệt, Q10",          DateOfBirth = new DateTime(1997, 9, 5),   DriverLicenseNo = "DL021234", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Vũ Lan Phương",    CCCD = "079206025678", PhoneNumber = "0978901234", Email = "phuong.vl@gmail.com",    Address = "34 Trần Hưng Đạo, Q1",           DateOfBirth = new DateTime(1992, 4, 18),  DriverLicenseNo = "DL025678", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Bùi Thanh Sơn",   CCCD = "079206029012", PhoneNumber = "0989012345", Email = "son.bt@gmail.com",       Address = "67 Cách Mạng Tháng 8, Q3",        DateOfBirth = new DateTime(1996, 12, 25), DriverLicenseNo = "DL029012", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Lý Mai Linh",     CCCD = "079206033456", PhoneNumber = "0901234567", Email = "linh.lm@gmail.com",      Address = "88 Nguyễn Huệ, Q1",               DateOfBirth = new DateTime(1994, 6, 10),  DriverLicenseNo = "DL033456", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
            new() { FullName = "Trịnh Ngọc Minh", CCCD = "079206037890", PhoneNumber = "0932123456", Email = "minh.tn@gmail.com",      Address = "21 Phạm Hồng Thái, Q1",          DateOfBirth = new DateTime(1991, 2, 28),  DriverLicenseNo = "DL037890", IsActive = true, CreatedAt = new DateTime(2026, 6, 1) },
        };
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();

        // --- 5. RentalContracts (5 sample) ---
        // Create contracts using entity references so EF sets FK automatically
        var contracts = new List<RentalContract>
        {
            new()
            {
                Id = 1, CustomerId = 1, MotorcycleId = 1,
                RentalDate = new DateTime(2026, 5, 25, 8, 0, 0, DateTimeKind.Utc),
                ExpectedReturnDate = new DateTime(2026, 5, 28, 20, 0, 0, DateTimeKind.Utc),
                ActualReturnDate = new DateTime(2026, 5, 28, 18, 0, 0, DateTimeKind.Utc),
                DailyRate = 100_000m, TotalAmount = 300_000m, DepositAmount = 200_000m, FinalAmount = 300_000m,
                Status = BusinessObjects.Enums.RentalStatus.Completed,
                StartMileage = 12000, EndMileage = 12150,
                Notes = "Trả xe đúng hạn, xe còn tốt", CreatedBy = "admin",
                IsActive = true, CreatedAt = new DateTime(2026, 5, 25)
            },
            new()
            {
                Id = 2, CustomerId = 3, MotorcycleId = 3,
                RentalDate = new DateTime(2026, 6, 1, 9, 0, 0, DateTimeKind.Utc),
                ExpectedReturnDate = new DateTime(2026, 6, 5, 20, 0, 0, DateTimeKind.Utc),
                DailyRate = 110_000m, TotalAmount = 440_000m, DepositAmount = 300_000m, FinalAmount = 0,
                Status = BusinessObjects.Enums.RentalStatus.Active,
                StartMileage = 15000,
                Notes = "Khách hàng du lịch Đà Lạt", CreatedBy = "admin",
                IsActive = true, CreatedAt = new DateTime(2026, 6, 1)
            },
            new()
            {
                Id = 3, CustomerId = 5, MotorcycleId = 8,
                RentalDate = new DateTime(2026, 6, 3, 8, 0, 0, DateTimeKind.Utc),
                ExpectedReturnDate = new DateTime(2026, 6, 6, 20, 0, 0, DateTimeKind.Utc),
                DailyRate = 180_000m, TotalAmount = 540_000m, DepositAmount = 400_000m, FinalAmount = 0,
                Status = BusinessObjects.Enums.RentalStatus.Pending,
                Notes = "Booking trước 1 tuần", CreatedBy = "staff",
                IsActive = true, CreatedAt = new DateTime(2026, 5, 30)
            },
            new()
            {
                Id = 4, CustomerId = 7, MotorcycleId = 2,
                RentalDate = new DateTime(2026, 5, 20, 8, 0, 0, DateTimeKind.Utc),
                ExpectedReturnDate = new DateTime(2026, 5, 22, 20, 0, 0, DateTimeKind.Utc),
                ActualReturnDate = new DateTime(2026, 5, 24, 14, 0, 0, DateTimeKind.Utc),
                DailyRate = 120_000m, TotalAmount = 240_000m, DepositAmount = 200_000m, FinalAmount = 360_000m,
                Status = BusinessObjects.Enums.RentalStatus.Completed,
                StartMileage = 8500, EndMileage = 8700,
                Notes = "Trả trễ 2 ngày, phí trễ đã tính", CreatedBy = "admin",
                IsActive = true, CreatedAt = new DateTime(2026, 5, 20)
            },
            new()
            {
                Id = 5, CustomerId = 9, MotorcycleId = 5,
                RentalDate = new DateTime(2026, 5, 28, 8, 0, 0, DateTimeKind.Utc),
                ExpectedReturnDate = new DateTime(2026, 5, 30, 20, 0, 0, DateTimeKind.Utc),
                DailyRate = 150_000m, TotalAmount = 300_000m, DepositAmount = 200_000m, FinalAmount = 0,
                Status = BusinessObjects.Enums.RentalStatus.Cancelled,
                Notes = "Khách hủy đột xuất do thời tiết xấu", CreatedBy = "staff",
                IsActive = true, CreatedAt = new DateTime(2026, 5, 27)
            },
        };
        await context.RentalContracts.AddRangeAsync(contracts);
        await context.SaveChangesAsync();
    }
}
