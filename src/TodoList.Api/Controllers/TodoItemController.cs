using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("/todo-item")]
    public class TodoItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemController(IMediator mediator) 
            => _mediator = mediator;

        [HttpPost]
        public async Task<Guid> Create([FromBody] CreateTodoItemCommand command)
        {
            var createdTodoItem = await _mediator.Send(command);

            // 處於演示的目的，這裡只返回創建出來的TodoItem的Id，理由同前
            return createdTodoItem;
        }
    }
}
