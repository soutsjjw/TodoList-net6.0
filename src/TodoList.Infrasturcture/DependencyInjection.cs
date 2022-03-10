using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoList.Application.Common.Interfaces;
using TodoList.Infrasturcture.Persistence;
using TodoList.Infrasturcture.Persistence.Repositories;
using TodoList.Infrasturcture.Services;

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

        // services.AddScoped<IApplicationDbContext>(
        //     provider => provider.GetRequiredService<TodoListDbContext>());

        // 增加依賴注入
        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
