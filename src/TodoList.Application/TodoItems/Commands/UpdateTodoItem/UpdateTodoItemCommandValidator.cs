using FluentValidation;
using TodoList.Application.Common.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator(IRepository<TodoItem> repository)
    {
        RuleFor(v => v.Title)
            .MaximumLength(10).WithMessage("TodoItem title must not exceed 10 characters.").WithSeverity(Severity.Warning)
            .NotEmpty().WithMessage("Title is required.").WithSeverity(Severity.Error);
    }
}

