using Api.ActionResult;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stap.Api.Helper;

namespace Api.Filter;

public class GlobalFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Response.StatusCode.IsSuccessStatusCode())
        {
            var result = context.Result;
            if (result is ObjectResult objectResult)
            {
                var response = new SuccessJsonResponse(objectResult.Value, MessageId.Success.GetEnumDescription());
                context.Result = new SuccessObjectResult(response, context.HttpContext.Response.StatusCode);
            }
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Nothing to do before action execution
    }
}