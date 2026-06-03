using Microsoft.AspNetCore.Authentication.Cookies;
using Services;
using Services.Interfaces;
using Repositories;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' was not found. Please configure user secrets or appsettings.");
}

// Add Entity Framework DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Repositories and Services
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

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Add Cookie Authentication for WebUI
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff", "Admin"));
    options.AddPolicy("AdminOrStaff", policy => policy.RequireRole("Admin", "Staff"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

app.Run();
