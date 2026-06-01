using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using Repositories;
using Services.Interfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories (Singleton)
builder.Services.AddSingleton<Repositories.IUserRepository, Repositories.UserRepository>();
builder.Services.AddSingleton<Repositories.IMotorcycleRepository, Repositories.MotorcycleRepository>();
builder.Services.AddSingleton<Repositories.IMotorcycleTypeRepository, Repositories.MotorcycleTypeRepository>();
builder.Services.AddSingleton<Repositories.ICustomerRepository, Repositories.CustomerRepository>();
builder.Services.AddSingleton<Repositories.IRentalRepository, Repositories.RentalRepository>();

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<IRentalService, RentalService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
