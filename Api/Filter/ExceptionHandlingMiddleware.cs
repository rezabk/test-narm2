using System.Net;
using Api.ActionResult;
using Common.ExceptionType.CustomException;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Api.Filter;

public class HttpGlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly IHostingEnvironment env;
    private readonly ILogger<HttpGlobalExceptionFilter> logger;


    public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
    {
        this.env = env;
        this.logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var logKey = new Random().Next();


        var receivedException = context.Exception.InnerException ?? context.Exception;

        logger.LogError(new EventId(logKey),
            receivedException,
            receivedException.Message);

        if (receivedException is ErrorException errorException)
        {
            var json = new ErrorJsonResponse(new[] { errorException.Message }, null);
            context.Result = new BadRequestObjectResult(json);
        }

        else if (receivedException is NotFoundException notFoundException)
        {
            var json = new ErrorJsonResponse(new[] { notFoundException.Message }, null);
            context.Result = new BadRequestObjectResult(json, (int)HttpStatusCode.NotFound);
        }

        else if (receivedException is ChangeStatusException changeStatusException)
        {
            var json = new ErrorJsonResponse(new[] { changeStatusException.Message }, null);
            context.Result = new BadRequestObjectResult(json, (int)HttpStatusCode.Forbidden);
        }

        else if (receivedException is CompleteProfileException completeProfileException)
        {
            var json = new ErrorJsonResponse(new[] { completeProfileException.Message }, null);
            context.Result = new BadRequestObjectResult(json, (int)HttpStatusCode.Forbidden);
        }

        else if (receivedException is ForbiddenException forbiddenException)
        {
            var json = new ErrorJsonResponse(new[] { forbiddenException.Message }, null);
            context.Result = new BadRequestObjectResult(json, (int)HttpStatusCode.Forbidden);
        }
       
        else if (receivedException is AccountIsLockedException accountIsLockedException)
        {
            var json = new ErrorJsonResponse(new[] { accountIsLockedException.Message }, null);
            context.Result = new BadRequestObjectResult(json, (int)HttpStatusCode.Locked);
        }
        else if (context.Exception is NoFlowException noFlowException)
        {
            var json = new ErrorJsonResponse(new[] { noFlowException.Message }, null);
            context.Result = new BadRequestObjectResult(json, (int)HttpStatusCode.Locked);
        }

        else if (receivedException is FormValidationException formValidationException)
        {
            object? helperMessage = null;
            var exception = formValidationException;
            if (exception.SpecificObject != null)
                helperMessage = exception.SpecificObject;
            else
                helperMessage = exception.HelperMessages;

            var json = new ErrorJsonResponse(new[] { exception.Message }, helperMessage);

            context.Result = new BadRequestObjectResult(json, (int)HttpStatusCode.UnprocessableEntity);
        }

        else
        {
            var json = new ErrorJsonResponse(new[] { "internal error occured " },
                env.IsDevelopment() ? JsonConvert.SerializeObject(receivedException) : null);
            context.Result = new InternalServerErrorObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }

    public void OnException(ExceptionContext context)
    {
        var a = 10;
    }
}