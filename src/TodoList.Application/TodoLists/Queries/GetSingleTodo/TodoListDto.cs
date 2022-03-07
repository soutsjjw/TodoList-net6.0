using TodoList.Application.Common.Mappings;
using TodoList.Application.TodoItems.Queries.GetTodoItems;

namespace TodoList.Application.TodoLists.Queries.GetSingleTodo;

// 實現IMapFrom<T>接口，因為此Dto不涉及特殊字段的Mapping規則
// 並且屬性名稱與領域實體保持一致，根據Convention規則默認可以完成Mapping，不需要額外實現
public class TodoListDto : IMapFrom<Domain.Entities.TodoList>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Colour { get; set; }

    public IList<TodoItemDto> Items { get; set; } = new List<TodoItemDto>();
}

