using TodoList.Api.Extensions;
using TodoList.Api.Filters;
using TodoList.Application;
using TodoList.Infrasturcture;
using TodoList.Infrasturcture.Log;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 配置日誌
builder.ConfigureLog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<LogFilterAttribute>();

// 添加應用層配置
builder.Services.AddApplication();

// 添加基礎設施配置
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// 全域異常處理
app.UseGlobalExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 調用擴展方法
app.MigrateDatabase();

app.Run();
