using Microsoft.EntityFrameworkCore;
using MobileExamAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(); // dùng Controller chuẩn MVC
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // dùng Swagger UI

// Kết nối DbContext tới SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapControllers(); // map tất cả các controller

app.Run();
