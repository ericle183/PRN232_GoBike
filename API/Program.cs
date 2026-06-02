using BusinessObjects.Entities;
using DataAccessObjects;
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

builder.Services.AddSingleton<IRepository<User>, UserRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IRepository<MotorcycleType>, MotorcycleTypeRepository>();
builder.Services.AddSingleton<IMotorcycleTypeRepository, MotorcycleTypeRepository>();
builder.Services.AddSingleton<IRepository<Motorcycle>, MotorcycleRepository>();
builder.Services.AddSingleton<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddSingleton<IRepository<Customer>, CustomerRepository>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IRepository<RentalContract>, RentalContractRepository>();
builder.Services.AddSingleton<IRentalContractRepository, RentalContractRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMotorcycleTypeService, MotorcycleTypeService>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRentalContractService, RentalContractService>();

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
app.UseAuthorization();
app.MapControllers();

app.Run();
