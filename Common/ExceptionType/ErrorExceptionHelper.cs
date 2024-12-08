using Common.Enums;

namespace Common.ExceptionType;

public static class ErrorExceptionHelper
{
    public static string CreateViewMessage(MessageId message, params object[] viewMessagePlaceHolders)
    {
        if (viewMessagePlaceHolders == null || viewMessagePlaceHolders.Length == 0)
            return message.GetEnumDescription();

        var viewMessage = message.GetEnumDescription();
        // .GetCustomAttribute<DisplayAttribute>()?.GetName();
        if (string.IsNullOrWhiteSpace(viewMessage)) viewMessage = message.ToString();
        if (viewMessagePlaceHolders.Length > 0) viewMessage = string.Format(viewMessage, viewMessagePlaceHolders);
        return viewMessage;
    }
}