using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace TodoList.Api.Filters;

public class LogFilterAttribute : IActionFilter
{
    private readonly ILogger<LogFilterAttribute> _logger;

    public LogFilterAttribute(ILogger<LogFilterAttribute> logger) => _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"];
        var controller = context.RouteData.Values["controller"];
        // 獲取名稱包含Command的參數值
        var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Command")).Value;

        _logger.LogInformation($"Controller:{controller}, action: {action}, Incoming request: {JsonSerializer.Serialize(param)}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var action = context.RouteData.Values["action"];
        var controller = context.RouteData.Values["controller"];
        // 需要先將Result轉換為ObjectResult類型才能拿到Value值
        var result = (ObjectResult)context.Result!;

        _logger.LogInformation($"Controller:{controller}, action: {action}, Executing response: {JsonSerializer.Serialize(result.Value)}");
    }
}

