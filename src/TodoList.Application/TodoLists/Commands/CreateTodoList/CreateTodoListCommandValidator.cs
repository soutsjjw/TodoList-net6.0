using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoList.Application.Common.Interfaces;

namespace TodoList.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    private readonly IRepository<Domain.Entities.TodoList> _repository;

    public CreateTodoListCommandValidator(IRepository<Domain.Entities.TodoList> repository)
    {
        _repository = repository;

        RuleFor(v => v.Title)
            .MaximumLength(100).WithMessage("TodoList title must not exceed 100 characters.").WithSeverity(Severity.Warning)
            .NotEmpty().WithMessage("Title is required.").WithSeverity(Severity.Error)
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.").WithSeverity(Severity.Warning);
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _repository.GetAsQueryable().AllAsync(l => l.Title != title, cancellationToken);
    }
}

