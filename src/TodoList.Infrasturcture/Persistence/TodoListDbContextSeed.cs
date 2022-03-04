using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using TodoList.Domain.ValueObjects;

namespace TodoList.Infrasturcture.Persistence;

public static class TodoListDbContextSeed
{
    public static async Task SeedSampleDataAsync(TodoListDbContext context)
    {
        if (!context.TodoLists.Any())
        {
            var list = new Domain.Entities.TodoList
            {
                Title = "Shopping",
                Colour = Colour.Blue
            };

            list.Items.Add(new TodoItem { Title = "Apples", Done = true, Priority = PriorityLevel.High });
            list.Items.Add(new TodoItem { Title = "Milk", Done = true });
            list.Items.Add(new TodoItem { Title = "Bread", Done = true });
            list.Items.Add(new TodoItem { Title = "Toilet paper" });
            list.Items.Add(new TodoItem { Title = "Pasta" });
            list.Items.Add(new TodoItem { Title = "Tissues" });
            list.Items.Add(new TodoItem { Title = "Tuna" });
            list.Items.Add(new TodoItem { Title = "Water" });

            context.TodoLists.Add(list);

            await context.SaveChangesAsync();
        }
    }

    public static async Task UpdateSampleDataAsync(TodoListDbContext context)
    {
        var sampleTodoList = await context.TodoLists.FirstOrDefaultAsync();
        if (sampleTodoList == null)
        {
            return;
        }

        sampleTodoList.Title = "Shopping - modified";

        // 演示更新時審計字段的變化
        context.Update(sampleTodoList);
        await context.SaveChangesAsync();
    }
}
