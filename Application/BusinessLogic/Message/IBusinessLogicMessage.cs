using Common.Enums;

namespace Application.BusinessLogic.Message;

public interface IPresentationMessage
{
    MessageType Type { get; }
    string ViewMessage { get; }
    int MessageCode { get; }
}

public interface IBusinessLogicMessage : IPresentationMessage
{
    MessageId Message { get; }
}