using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' was not found. Please configure user secrets or appsettings.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMotorcycleTypeRepository, MotorcycleTypeRepository>();
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRentalContractRepository, RentalContractRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMotorcycleTypeService, MotorcycleTypeService>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRentalContractService, RentalContractService>();

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.AccessDeniedPath = "/api/auth/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff", "Admin"));
    options.AddPolicy("AdminOrStaff", policy => policy.RequireRole("Admin", "Staff"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    await DBSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
