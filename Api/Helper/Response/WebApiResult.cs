using System.Net;
using Application.BusinessLogic;
using Application.BusinessLogic.Message;

namespace Stap.Api.Helper.Response;

public class WebApiResult
{
    public int MessageCode { get; set; }
    public List<string> Message { get; set; }
    public MessageViewModel Exception { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public List<string> ErrorFileds { get; set; }

    public Guid TraceId { get; set; }
}

public class WebApiResult<TResult> : WebApiResult
{
    public TResult Result { get; set; }
}

public static class WebApiResultClass
{
    public static WebApiResult ToWebApiResult(this IBusinessLogicResult source)
    {
        var result = new WebApiResult();
        result.Exception = source.Exception.GetExceptionMessage();
        result.HttpStatusCode = source.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        result.MessageCode = source.Messages.FirstOrDefault(x => x.Type == MessageType.Error)?.MessageCode ?? 0;
        result.Message = source.Messages.Select(x => x.ViewMessage).ToList();
        result.ErrorFileds = source.ErrorFileds.ToList();
        return result;
    }

    public static WebApiResult<TResult> ToWebApiResult<TResult>(this IBusinessLogicResult<TResult> source)
    {
        var result = new WebApiResult<TResult>();
        result.Result = source.Result;
        result.Exception = source.Exception.GetExceptionMessage();
        result.HttpStatusCode = source.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        result.MessageCode = source.Messages.FirstOrDefault(x => x.Type == MessageType.Error)?.MessageCode ?? 0;
        result.Message = source.Messages.Select(x => x.ViewMessage).ToList();
        result.ErrorFileds = source.ErrorFileds.ToList();
        return result;
    }
}