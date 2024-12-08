using Microsoft.AspNetCore.Mvc;
using Stap.Api.Helper;

namespace Api.ActionResult;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object value) : base(value)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

public class BadRequestObjectResult : ObjectResult
{
    public BadRequestObjectResult(object value, int statusCode = StatusCodes.Status400BadRequest) : base(value)
    {
        StatusCode = statusCode;
    }
}

public class SuccessObjectResult : ObjectResult
{
    public SuccessObjectResult(object value, int statusCode = 200) : base(value)
    {
        StatusCode = statusCode.IsSuccessStatusCode() ? StatusCodes.Status200OK : statusCode;
    }
}