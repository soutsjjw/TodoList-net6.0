using MediatR;
using TodoList.Application.Common.Exceptions;
using TodoList.Application.Common.Interfaces;

namespace TodoList.Application.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTodoListCommandhandler : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IRepository<Domain.Entities.TodoList> _repository;

    public DeleteTodoListCommandhandler(IRepository<Domain.Entities.TodoList> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoList), request.Id);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        // 對於Delete操作，演示中並不返回任何實際的物件，可以結合實際需要返回特定的物件。 Unit物件在MediatR中表示Void
        return Unit.Value;
    }
}

