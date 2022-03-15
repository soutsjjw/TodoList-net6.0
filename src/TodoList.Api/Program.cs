using Microsoft.AspNetCore.HttpLogging;
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

builder.Services.AddHttpLogging(options =>
{
    // 日誌紀錄的字段配置，可以以 | 連接
    options.LoggingFields = HttpLoggingFields.All;

    // 增加請求header紀錄
    options.RequestHeaders.Add("Sec-Fetch-Site");
    options.RequestHeaders.Add("Sec-Fetch-Mode");
    options.RequestHeaders.Add("Sec-Fetch-Dest");

    // 增加回應header紀錄
    options.ResponseHeaders.Add("Server");

    // 增加回應的媒體類型
    options.MediaTypeOptions.AddText("application/javascript");

    // 配置請求body日誌最大長度
    options.RequestBodyLogLimit = 4096;

    // 配置回應body日誌最大長度
    options.ResponseBodyLogLimit = 4096;
});


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

app.UseHttpLogging();

app.MapControllers();

// 調用擴展方法
app.MigrateDatabase();

app.Run();
