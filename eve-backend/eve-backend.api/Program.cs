using eve_backend.data;
using Microsoft.EntityFrameworkCore;
using eve_backend.logic.Interfaces;
using eve_backend.logic.Services;
using eve_backend.data.Repositories;
using eve_backend.logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IExcelRepository, ExcelRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IObjectRepository, ObjectRepository>();
builder.Services.AddScoped<IObjectService, ObjectService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "EVE",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
}); builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionstring, b => b.MigrationsAssembly("eve-backend.api"));
});
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("EVE");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
