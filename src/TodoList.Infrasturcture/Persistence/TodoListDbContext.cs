using Microsoft.EntityFrameworkCore;
using TodoList.Application.Common.Interfaces;

namespace TodoList.Infrasturcture.Persistence;

public class TodoListDbContext: DbContext, IApplicationDbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
    {
        
    }
}
