using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Infrasturcture.Persistence;

namespace TodoList.Infrasturcture;

public static class ApplicationStartupExtensions
{
    public static async void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<TodoListDbContext>();
            context.Database.Migrate();

            // 生成種子數據
            TodoListDbContextSeed.SeedSampleDataAsync(context).Wait();

            // 更新部分種子數據以便查看審計字段
            TodoListDbContextSeed.UpdateSampleDataAsync(context).Wait();
        }
        catch (System.Exception ex)
        {
            throw new Exception($"An error occurred migrating the DB: {ex.Message}");
        }
    }
}
