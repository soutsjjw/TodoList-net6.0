using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Models;
using TodoList.Domain.Events;

namespace TodoList.Application.TodoItems.EventHandlers;

public class TodoItemCompletedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCompletedEvent>>
{
    private readonly ILogger<TodoItemCompletedEventHandler> _logger;

    public TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TodoItemCompletedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // 這裡我們還是只做日誌輸出，實際使用中根據需要進行業務邏輯處理，但是在Handler中不建議繼續Send其他Command或Notification
        _logger.LogInformation("TodoList Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
