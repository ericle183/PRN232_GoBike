using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessObjects.Migrations
{
    /// <inheritdoc />
    public partial class RequirementV1Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "DailyRate",
                table: "Motorcycles");

            migrationBuilder.RenameColumn(
                name: "StartMileage",
                table: "RentalContracts",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameColumn(
                name: "RentalDate",
                table: "RentalContracts",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "ExpectedReturnDate",
                table: "RentalContracts",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "EndMileage",
                table: "RentalContracts",
                newName: "NoShowByUserId");

            migrationBuilder.RenameColumn(
                name: "DailyRate",
                table: "RentalContracts",
                newName: "RemainingAmount");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalPaymentAmount",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CancellationFee",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "RentalContracts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "RentalContracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancelledByUserId",
                table: "RentalContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "RentalContracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletedByUserId",
                table: "RentalContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "RentalContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyPrice",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DamageDescription",
                table: "RentalContracts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DamageFee",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DiscountReason",
                table: "RentalContracts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LateDays",
                table: "RentalContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "LateFee",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "NoShowAt",
                table: "RentalContracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoShowReason",
                table: "RentalContracts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherFee",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "OtherFeeDescription",
                table: "RentalContracts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RefundAmount",
                table: "RentalContracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RentalDays",
                table: "RentalContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MotorcycleTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultDepositAmount",
                table: "MotorcycleTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MotorcycleTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MotorcycleId = table.Column<int>(type: "int", nullable: false),
                    RentalContractId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RepairCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Motorcycles_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_RentalContracts_RentalContractId",
                        column: x => x.RentalContractId,
                        principalTable: "RentalContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RentalInspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalContractId = table.Column<int>(type: "int", nullable: false),
                    InspectionType = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    FuelLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleCondition = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HasDamage = table.Column<bool>(type: "bit", nullable: false),
                    DamageDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AccessoriesNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalInspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalInspections_RentalContracts_RentalContractId",
                        column: x => x.RentalContractId,
                        principalTable: "RentalContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalInspections_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RentalPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalContractId = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalPayments_RentalContracts_RentalContractId",
                        column: x => x.RentalContractId,
                        principalTable: "RentalContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalPayments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_CancelledByUserId",
                table: "RentalContracts",
                column: "CancelledByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_CompletedByUserId",
                table: "RentalContracts",
                column: "CompletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_CreatedByUserId",
                table: "RentalContracts",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_NoShowByUserId",
                table: "RentalContracts",
                column: "NoShowByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_UpdatedByUserId",
                table: "RentalContracts",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleTypes_Name",
                table: "MotorcycleTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_RegistrationNo",
                table: "Motorcycles",
                column: "RegistrationNo",
                unique: true,
                filter: "[RegistrationNo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DriverLicenseNo",
                table: "Customers",
                column: "DriverLicenseNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_CreatedByUserId",
                table: "MaintenanceRecords",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_MotorcycleId",
                table: "MaintenanceRecords",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_RentalContractId",
                table: "MaintenanceRecords",
                column: "RentalContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_UpdatedByUserId",
                table: "MaintenanceRecords",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalInspections_CreatedByUserId",
                table: "RentalInspections",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalInspections_RentalContractId_InspectionType",
                table: "RentalInspections",
                columns: new[] { "RentalContractId", "InspectionType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalPayments_CreatedByUserId",
                table: "RentalPayments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalPayments_RentalContractId",
                table: "RentalPayments",
                column: "RentalContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Users_CancelledByUserId",
                table: "RentalContracts",
                column: "CancelledByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Users_CompletedByUserId",
                table: "RentalContracts",
                column: "CompletedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Users_CreatedByUserId",
                table: "RentalContracts",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Users_NoShowByUserId",
                table: "RentalContracts",
                column: "NoShowByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_Users_UpdatedByUserId",
                table: "RentalContracts",
                column: "UpdatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Users_CancelledByUserId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Users_CompletedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Users_CreatedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Users_NoShowByUserId",
                table: "RentalContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_Users_UpdatedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "RentalInspections");

            migrationBuilder.DropTable(
                name: "RentalPayments");

            migrationBuilder.DropIndex(
                name: "IX_RentalContracts_CancelledByUserId",
                table: "RentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_RentalContracts_CompletedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_RentalContracts_CreatedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_RentalContracts_NoShowByUserId",
                table: "RentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_RentalContracts_UpdatedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_MotorcycleTypes_Name",
                table: "MotorcycleTypes");

            migrationBuilder.DropIndex(
                name: "IX_Motorcycles_RegistrationNo",
                table: "Motorcycles");

            migrationBuilder.DropIndex(
                name: "IX_Customers_DriverLicenseNo",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdditionalPaymentAmount",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CancellationFee",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CancelledByUserId",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CompletedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "DailyPrice",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "DamageDescription",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "DamageFee",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "DiscountReason",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "LateDays",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "LateFee",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "NoShowAt",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "NoShowReason",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "OtherFee",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "OtherFeeDescription",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "RefundAmount",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "RentalDays",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MotorcycleTypes");

            migrationBuilder.DropColumn(
                name: "DefaultDepositAmount",
                table: "MotorcycleTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MotorcycleTypes");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "RentalContracts",
                newName: "StartMileage");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "RentalContracts",
                newName: "RentalDate");

            migrationBuilder.RenameColumn(
                name: "RemainingAmount",
                table: "RentalContracts",
                newName: "DailyRate");

            migrationBuilder.RenameColumn(
                name: "NoShowByUserId",
                table: "RentalContracts",
                newName: "EndMileage");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "RentalContracts",
                newName: "ExpectedReturnDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RentalContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyRate",
                table: "Motorcycles",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
