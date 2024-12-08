using Application.BusinessLogic.Message;

namespace Application.BusinessLogic;

public class BusinessLogicResult : IBusinessLogicResult
{
    public BusinessLogicResult(bool succeeded, IEnumerable<IBusinessLogicMessage> messages = null,
        Exception exception = null, IEnumerable<string> errorFileds = null)
    {
        Succeeded = succeeded;
        Exception = exception;
        Messages = new List<IPresentationMessage>();
        if (messages == null) return;
        foreach (var message in messages) Messages.Add(message);

        ErrorFileds = new List<string>();
        if (errorFileds == null) return;
        foreach (var error in errorFileds) ErrorFileds.Add(error);
    }

    public bool Succeeded { get; }
    public IList<IPresentationMessage> Messages { get; }
    public Exception Exception { get; }
    public IList<string> ErrorFileds { get; }
}

public class BusinessLogicResult<TResult> : BusinessLogicResult, IBusinessLogicResult<TResult>
{
    public BusinessLogicResult(bool succeeded, TResult result, IEnumerable<IBusinessLogicMessage> messages = null,
        Exception exception = null, int pageCount = 0, IEnumerable<string> errorFileds = null)
        : base(succeeded, messages, exception, errorFileds)
    {
        Result = result;
        PageCount = pageCount;
    }

    public TResult Result { get; }
    public int PageCount { get; }
}