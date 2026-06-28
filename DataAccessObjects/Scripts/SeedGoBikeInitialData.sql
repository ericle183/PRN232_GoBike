SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- Ensure the image column exists because motorcycle inserts include ImageUrl.
IF COL_LENGTH('dbo.Motorcycles', 'ImageUrl') IS NULL
BEGIN
    ALTER TABLE dbo.Motorcycles
    ADD ImageUrl nvarchar(500) NULL;
END;

-- Default login accounts:
-- admin / admin123
-- staff / staff123
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'admin')
BEGIN
    INSERT INTO dbo.Users
        (Username, PasswordHash, FullName, Email, PhoneNumber, Role, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'admin',
         N'$2a$11$lMqfmiF3EgZwarF3lGK51e468xYPMGok7Y8EORMpbap3bnSQifizu',
         N'Admin User',
         N'admin@gobike.local',
         N'0900000001',
         1,
         1,
         SYSUTCDATETIME(),
         NULL);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'staff')
BEGIN
    INSERT INTO dbo.Users
        (Username, PasswordHash, FullName, Email, PhoneNumber, Role, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'staff',
         N'$2a$11$A3f7f8EaEqUDjYFlrSDuJuWb5dz5q3nc5AKrwArSG8zGTIyifUpbm',
         N'Staff User',
         N'staff@gobike.local',
         N'0900000002',
         2,
         1,
         SYSUTCDATETIME(),
         NULL);
END;

-- Customer profiles. Customers do not log in; they are renter records.
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE CCCD = N'079206000001')
BEGIN
    INSERT INTO dbo.Customers
        (FullName, CCCD, PhoneNumber, Email, Address, DateOfBirth, DriverLicenseNo, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'Nguyễn Văn Minh',
         N'079206000001',
         N'0912345678',
         N'minh.nguyen@example.com',
         N'Quận 1, TP.HCM',
         '1996-04-12',
         N'GPLX000001',
         1,
         SYSUTCDATETIME(),
         NULL);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE CCCD = N'079206000002')
BEGIN
    INSERT INTO dbo.Customers
        (FullName, CCCD, PhoneNumber, Email, Address, DateOfBirth, DriverLicenseNo, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'Trần Hoàng Anh',
         N'079206000002',
         N'0923456789',
         N'anh.tran@example.com',
         N'Quận Bình Thạnh, TP.HCM',
         '1994-09-25',
         N'GPLX000002',
         1,
         SYSUTCDATETIME(),
         NULL);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE CCCD = N'079206000003')
BEGIN
    INSERT INTO dbo.Customers
        (FullName, CCCD, PhoneNumber, Email, Address, DateOfBirth, DriverLicenseNo, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'Lê Mai Phương',
         N'079206000003',
         N'0934567890',
         N'phuong.le@example.com',
         N'Thủ Đức, TP.HCM',
         '1998-01-18',
         N'GPLX000003',
         1,
         SYSUTCDATETIME(),
         NULL);
END;

-- Motorcycle types.
IF NOT EXISTS (SELECT 1 FROM dbo.MotorcycleTypes WHERE Name = N'Xe số')
BEGIN
    INSERT INTO dbo.MotorcycleTypes
        (Name, Description, DefaultDailyRate, DefaultDepositAmount, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'Xe số',
         N'Xe số phổ thông, tiết kiệm xăng, dễ vận hành.',
         100000,
         500000,
         1,
         SYSUTCDATETIME(),
         NULL);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.MotorcycleTypes WHERE Name = N'Xe ga')
BEGIN
    INSERT INTO dbo.MotorcycleTypes
        (Name, Description, DefaultDailyRate, DefaultDepositAmount, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'Xe ga',
         N'Xe tay ga tiện dụng cho di chuyển trong thành phố.',
         150000,
         800000,
         1,
         SYSUTCDATETIME(),
         NULL);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.MotorcycleTypes WHERE Name = N'Xe côn')
BEGIN
    INSERT INTO dbo.MotorcycleTypes
        (Name, Description, DefaultDailyRate, DefaultDepositAmount, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (N'Xe côn',
         N'Xe côn tay dành cho khách có kinh nghiệm lái.',
         220000,
         1000000,
         1,
         SYSUTCDATETIME(),
         NULL);
END;

DECLARE @XeSoId int = (SELECT TOP (1) Id FROM dbo.MotorcycleTypes WHERE Name = N'Xe số');
DECLARE @XeGaId int = (SELECT TOP (1) Id FROM dbo.MotorcycleTypes WHERE Name = N'Xe ga');
DECLARE @XeConId int = (SELECT TOP (1) Id FROM dbo.MotorcycleTypes WHERE Name = N'Xe côn');

DECLARE @Motorcycles TABLE
(
    LicensePlate nvarchar(20) NOT NULL,
    Brand nvarchar(50) NOT NULL,
    Model nvarchar(50) NOT NULL,
    VehicleTypeId int NOT NULL,
    Color nvarchar(30) NOT NULL,
    Mileage int NOT NULL,
    RegistrationNo nvarchar(20) NULL,
    ImageUrl nvarchar(500) NULL
);

INSERT INTO @Motorcycles
    (LicensePlate, Brand, Model, VehicleTypeId, Color, Mileage, RegistrationNo, ImageUrl)
VALUES
    -- Xe số: 6 xe
    (N'51A-12001', N'Honda',  N'Wave Alpha', @XeSoId,  N'Đen',   12500, N'REG-XS-001', NULL),
    (N'51A-12002', N'Honda',  N'Future',     @XeSoId,  N'Bạc',    8200, N'REG-XS-002', NULL),
    (N'51A-12003', N'Yamaha', N'Sirius',     @XeSoId,  N'Đỏ',     9400, N'REG-XS-003', NULL),
    (N'51A-12004', N'Yamaha', N'Jupiter',    @XeSoId,  N'Xanh',   7600, N'REG-XS-004', NULL),
    (N'51A-12005', N'Honda',  N'Blade',      @XeSoId,  N'Trắng', 11200, N'REG-XS-005', NULL),
    (N'51A-12006', N'Suzuki', N'Viva',       @XeSoId,  N'Đen',   15800, N'REG-XS-006', NULL),

    -- Xe ga: 6 xe
    (N'51G-22001', N'Honda',   N'Vision',   @XeGaId,  N'Trắng',  5300, N'REG-XG-001', NULL),
    (N'51G-22002', N'Honda',   N'Lead',     @XeGaId,  N'Đỏ',     6100, N'REG-XG-002', NULL),
    (N'51G-22003', N'Honda',   N'Air Blade',@XeGaId,  N'Đen',    7800, N'REG-XG-003', NULL),
    (N'51G-22004', N'Yamaha',  N'Grande',   @XeGaId,  N'Xám',    4200, N'REG-XG-004', NULL),
    (N'51G-22005', N'Yamaha',  N'Janus',    @XeGaId,  N'Xanh',   6600, N'REG-XG-005', NULL),
    (N'51G-22006', N'Piaggio', N'Liberty',  @XeGaId,  N'Trắng',  3500, N'REG-XG-006', NULL),

    -- Xe côn: 5 xe
    (N'51C-32001', N'Yamaha',   N'Exciter 155', @XeConId, N'Xanh',  4800, N'REG-XC-001', NULL),
    (N'51C-32002', N'Honda',    N'Winner X',    @XeConId, N'Đỏ',    5700, N'REG-XC-002', NULL),
    (N'51C-32003', N'Suzuki',   N'Raider',      @XeConId, N'Đen',   6900, N'REG-XC-003', NULL),
    (N'51C-32004', N'Yamaha',   N'MT-15',       @XeConId, N'Xám',   3100, N'REG-XC-004', NULL),
    (N'51C-32005', N'Kawasaki', N'Z125',        @XeConId, N'Xanh',  2900, N'REG-XC-005', NULL);

INSERT INTO dbo.Motorcycles
    (LicensePlate, Brand, Model, VehicleTypeId, Status, Color, Mileage, RegistrationNo, ImageUrl, IsActive, CreatedAt, UpdatedAt)
SELECT
    m.LicensePlate,
    m.Brand,
    m.Model,
    m.VehicleTypeId,
    1,
    m.Color,
    m.Mileage,
    m.RegistrationNo,
    m.ImageUrl,
    1,
    SYSUTCDATETIME(),
    NULL
FROM @Motorcycles m
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Motorcycles existing
    WHERE existing.LicensePlate = m.LicensePlate
);

COMMIT TRANSACTION;
