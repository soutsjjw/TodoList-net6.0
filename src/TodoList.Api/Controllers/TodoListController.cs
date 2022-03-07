using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.TodoLists.Commands.CreateTodoList;

namespace TodoList.Api.Controllers;

[ApiController]
[Route("/todo-list")]
public class TodoListController : ControllerBase
{
    private readonly IMediator _mediator;

    // 注入 MediatR
    public TodoListController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<Guid> Create([FromBody] CreateTodoListCommand command)
    {
        var createdTodoListId = await _mediator.Send(command);

        // 出於演示的目的，這裡只返回創建出來的TodoList的Id，
        // 實際使用中可能會選擇IActionResult作為返回的類型並返回CreatedAtRoute對象，
        // 因為我們還沒有去寫GET方法，返回CreatedAtRoute會報錯（找不到對應的Route），等講完GET後會在那裡更新
        return createdTodoListId;
    }
}
