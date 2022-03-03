using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Common.Interfaces;
using TodoList.Infrasturcture.Persistence;

namespace TodoList.Infrasturcture;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TodoListDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServerConnection"),
                b => b.MigrationsAssembly(typeof(TodoListDbContext).Assembly.FullName)
            ));

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<TodoListDbContext>());
        
        return services;
    }
}
