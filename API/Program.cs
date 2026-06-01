using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using Repositories;
using Services.Interfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// Repositories (Singleton pattern as per AGENTS.md)
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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
