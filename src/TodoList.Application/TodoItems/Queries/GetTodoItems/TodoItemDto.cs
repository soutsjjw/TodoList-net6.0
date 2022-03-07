using AutoMapper;
using TodoList.Application.Common.Mappings;
using TodoList.Domain.Entities;

namespace TodoList.Application.TodoItems.Queries.GetTodoItems;

// 實現IMapFrom<T>接口
public class TodoItemDto : IMapFrom<TodoItem>
{
    public Guid Id { get; set; }
    public Guid ListId { get; set; }
    public string? Title { get; set; }
    public bool Done { get; set; }
    public int Priority { get; set; }

    // 實現接口定義的Mapping方法，並提供除了Convention之外的特殊字段的轉換規則
    public void Mapping(Profile profile)
    {
        profile.CreateMap<TodoItem, TodoItemDto>()
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority));
    }
}

