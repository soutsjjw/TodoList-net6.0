using TodoList.Application.Common;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;

namespace TodoList.Application.TodoItems.Specs;

public class TodoItemSpec : SpecificationBase<TodoItem>
{
    public TodoItemSpec(bool done, PriorityLevel priority) : base(t => t.Done == done && t.Priority == priority)
    { }
}
