using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Common.Mappings;
using TodoList.Application.Common.Models;
using TodoList.Application.TodoItems.Specs;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;

namespace TodoList.Application.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsWithConditionQuery : IRequest<PaginatedList<TodoItemDto>>
{
    public Guid ListId { get; set; }
    public bool? Done { get; set; }
    public string? Title { get; set; }
    public PriorityLevel? PriorityLevel { get; set; }
    public string? SortOrder { get; set; } = "title_asc";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetTodoItemsWithConditionQueryHandler : IRequestHandler<GetTodoItemsWithConditionQuery, PaginatedList<TodoItemDto>>
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IMapper _mapper;

    public GetTodoItemsWithConditionQueryHandler(IRepository<TodoItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TodoItemDto>> Handle(GetTodoItemsWithConditionQuery request, CancellationToken cancellationToken)
    {
        var spec = new TodoItemSpec(request);
        return await _repository
            .GetAsQueryable(spec)
            .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
