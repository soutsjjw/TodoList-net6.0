using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoList.Application.Common.Interfaces;
using TodoList.Domain.Base;
using TodoList.Domain.Base.Interface;
using TodoList.Domain.Entities;

namespace TodoList.Infrasturcture.Persistence;

public class TodoListDbContext : DbContext
{
    private readonly IDomainEventService _domainEventService;

    public TodoListDbContext(DbContextOptions<TodoListDbContext> options,
        IDomainEventService domainEventService) : base(options)
    {
        _domainEventService = domainEventService;
    }

    public DbSet<Domain.Entities.TodoList> TodoLists => Set<Domain.Entities.TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach(var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch(entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = "Anonymous";
                    entry.Entity.Created = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = "Anonymous";
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }

        // 在寫數據庫的時候同時發送領域事件，這裡要注意一定要保證寫入數據庫成功後再發送領域事件，否則會導致領域對象狀態的不一致問題。
        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();
        
        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // 應用當前Assembly中定義的所有的Configurations，就不需要一個一個去寫了。
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach(var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}
