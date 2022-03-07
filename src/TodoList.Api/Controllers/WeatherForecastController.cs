using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.TodoItems.Specs;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;

namespace TodoList.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IRepository<TodoItem> _repository;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(IRepository<TodoItem> repository, ILogger<WeatherForecastController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // 紀錄日誌
        _logger.LogInformation($"maybe this log is provided by Serilog...");

        var spec = new TodoItemSpec(true, PriorityLevel.High);
        var items = _repository.GetAsync(spec).Result;

        foreach(var item in items)
        {
            _logger.LogInformation($"item: {item.Id} - {item.Title} - {item.Priority}");
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
