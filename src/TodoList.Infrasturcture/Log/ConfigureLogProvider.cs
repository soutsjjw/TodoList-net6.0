using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace TodoList.Infrasturcture.Log;

public static class ConfigureLogProvider
{
    public static void ConfigureLog(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool>("UseFileToLog"))
        {
            // 配置同時輸出到控制台和文件，並且指定文件名和文件轉儲方式（形如log-20211219.txt格式），轉儲文件保留的天數為15天，以及日誌格式
            // 配置Enrich.FromLogContext()的目的是為了從日誌上下文中獲取一些關鍵信息諸如用戶ID或請求ID，我們的應用中暫時不使用這些。
            Serilog.Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/log-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 15)
                .CreateLogger();
        }
        else
        {
            // 僅配置控制台日誌
            Serilog.Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        // 使用Serilog作為日誌框架，注意這里和.NET 5及之前的版本寫法是不太一樣的。
        builder.Host.UseSerilog();
    }
}
