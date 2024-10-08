using eve_backend.data;
using Microsoft.EntityFrameworkCore;
using eve_backend.logic;
using eve_backend.logic.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExcelService, ExcelService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionstring);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
