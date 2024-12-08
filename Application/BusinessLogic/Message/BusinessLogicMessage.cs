using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Common.Enums;

namespace Application.BusinessLogic.Message;

public abstract class BusinessLogicMessageBase : IPresentationMessage
{
    internal BusinessLogicMessageBase(MessageType type, string viewMessage, int messageCode)
    {
        Type = type;
        ViewMessage = viewMessage;
        MessageCode = messageCode;
    }

    public MessageType Type { get; }

    public string ViewMessage { get; }

    public int MessageCode { get; }
}

public class BusinessLogicMessage : BusinessLogicMessageBase, IBusinessLogicMessage
{
    public BusinessLogicMessage(MessageType type, MessageId message, params string[] viewMessagePlaceHolders)
        : base(type, CreateViewMessage(message, viewMessagePlaceHolders), (int)message)
    {
        Message = message;
    }

    public MessageId Message { get; }

    private static string CreateViewMessage(MessageId message, params string[] viewMessagePlaceHolders)
    {
        var viewMessage = message.GetType().GetMember(message.ToString()).First()
            .GetCustomAttribute<DisplayAttribute>()?.GetName();
        if (string.IsNullOrWhiteSpace(viewMessage)) viewMessage = message.ToString();
        if (viewMessagePlaceHolders != null && viewMessagePlaceHolders.Length > 0)
            viewMessage = string.Format(viewMessage, viewMessagePlaceHolders);
        return viewMessage;
    }
}