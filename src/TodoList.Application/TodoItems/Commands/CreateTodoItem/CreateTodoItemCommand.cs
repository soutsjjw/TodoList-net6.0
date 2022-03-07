using MediatR;
using TodoList.Application.Common.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommand : IRequest<Guid>
{
    public Guid ListId { get; set; }

    public string? Title { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Guid>
{
    private readonly IRepository<TodoItem> _repository;

    public CreateTodoItemCommandHandler(IRepository<TodoItem> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        await _repository.AddAsync(entity, cancellationToken);

        return entity.Id;
    }
}
