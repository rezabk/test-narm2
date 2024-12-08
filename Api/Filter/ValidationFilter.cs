


using System.Net;
using Api.ActionResult;
using Common.Enums;
using Common.ExceptionType.CustomException;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace  Api.Filter;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

            var errors = errorsInModelState.Select(x => new ErrorViewModel
            {
                Key = x.Key,
                Messages = x.Value
            }).ToList();
            throw new FormValidationException(MessageId.FormValidationExceptionOccured, errors);
        }

        await next();
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            // Construct a custom response for validation errors
            var errors = validationException.Errors.Select(error => new
            {
                error.PropertyName, error.ErrorMessage
            }).ToList();

            var response = new ValidationFailedJsonResponse(errors, "Validation failed");
            context.Result = new BadRequestObjectResult(response);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.ExceptionHandled = true;
        }

        return Task.CompletedTask;
    }
}