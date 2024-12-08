using Application.BusinessLogic.Message;

namespace Application.BusinessLogic;

public interface IBusinessLogicResult
{
    bool Succeeded { get; }
    IList<IPresentationMessage> Messages { get; }
    Exception Exception { get; }
    public IList<string> ErrorFileds { get; }
}

public interface IBusinessLogicResult<out TResult> : IBusinessLogicResult
{
    TResult Result { get; }
    int PageCount { get; }
}