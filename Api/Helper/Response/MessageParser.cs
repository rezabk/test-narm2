using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Application.BusinessLogic.Message;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Stap.Api.Helper.Response;

#region Classes

/// <summary>
///     کلاس مدیریت پیغام ها
/// </summary>
public static class MessageParser
{
    #region Methods

    //==========================================================  To Client Message
    /// <summary>
    ///     با لیستی از PresentationMessage , old message
    /// </summary>
    /// <param name="presentationMessages"></param>
    /// <returns></returns>
    public static List<MessageViewModel> ToClientMessage(this IList<IPresentationMessage> presentationMessages,
        List<MessageViewModel> oldMessages)
    {
        var newMessages = presentationMessages.Select(p => p.ToClientMessage()).ToList();
        if (oldMessages != null)
            newMessages.AddRange(oldMessages);
        return newMessages;
    }

    /// <summary>
    ///     با لیستی از PresentationMessage
    /// </summary>
    /// <param name="presentationMessages"></param>
    /// <returns></returns>
    public static List<MessageViewModel> ToClientMessage(this IList<IPresentationMessage> presentationMessages)
    {
        var newMessages = presentationMessages.Select(p => p.ToClientMessage()).ToList();
        return newMessages;
    }

    /// <summary>
    ///     با یک ورودی PresentationMessage
    /// </summary>
    /// <param name="presentationMessage"></param>
    /// <returns></returns>
    public static MessageViewModel ToClientMessage(this IPresentationMessage presentationMessage)
    {
        return new MessageViewModel
        {
            Message = presentationMessage.ViewMessage,
            Type = presentationMessage.Type.ToString().ToLower()
        };
    }

    /// <summary>
    ///     با یک ورودی مدل استیت
    /// </summary>
    /// <param name="modelstate"></param>
    /// <returns></returns>
    public static List<MessageViewModel> ToClientMessage(this ModelStateDictionary modelstate)
    {
        var _MessageViewModel = new List<MessageViewModel>();
        foreach (var item in modelstate)
            if (item.Value.Errors.Count > 0)
                for (var i = 0; i < item.Value.Errors.Count; i++)
                    _MessageViewModel.Add(new MessageViewModel
                    {
                        Message = item.Value.Errors[i].ErrorMessage,
                        Type = MessageType.Error.ToString()
                    });
        return _MessageViewModel;
    }

    /// <summary>
    ///     با یک ورودی نوع ارور که به پیغام دستی می چسبد
    /// </summary>
    /// <param name="message"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<MessageViewModel> ToClientMessage(this string message, MessageType type)
    {
        return new List<MessageViewModel>
        {
            new()
            {
                Type = type.ToPersianType(),
                Message = message.ToPersianMessage()
            }
        };
    }

    //==========================================================  To Persian Message
    /// <summary>
    ///     بدون ورودی که به متن پیغام می چسبد
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static string ToPersianMessage(this string message)
    {
        if (message.Contains("field is required.")) message = "field is required.";

        switch (message)
        {
            case "Parameter is not valid.":
                return "پارامترهای ارسالی نامعتبر است.";

            case "A generic error occurred in GDI+.":
                return "";

            case "Attempted to divide by zero.":
                return "خطای تقسیم بر صفر";

            case "Argument null exception.":
                return "پارامتر ارسالی نباید خالی باشد.";

            case "field is required.":
                return "پر کردن فیلد الزامی می باشد.";

            default:
                return message;
        }

        //The Price field is required.
    }

    //==========================================================  To Persian Type
    /// <summary>
    ///     بدون ورودی که به نوع ارور می چسبد
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string ToPersianType(this Enum type)
    {
        return type.GetType()
            .GetMember(type.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            .GetName();
    }

    internal static MessageViewModel GetExceptionMessage(this Exception exception)
    {
        if (exception == null) return null;
        var message = new StringBuilder("- ");
        message.AppendLine(exception.Message ?? exception.StackTrace);
        var innerException = exception.InnerException;
        while (innerException != null)
        {
            message.Append("- ");
            message.AppendLine(innerException.Message ?? innerException.StackTrace);
            innerException = innerException.InnerException;
        }

        message.Append("- ");
        message.AppendLine(exception.StackTrace ?? "");
        return new MessageViewModel
        {
            Message = message.ToString(),
            Type = MessageType.Error.ToString()
        };
        //return exception == null ? null : new MessageViewModel { Message = exception.Message ?? exception.InnerException.Message ?? exception.StackTrace, Type = MessageTypeViewModel.Error.GetDisplayName() };
    }

    #endregion Methods
}

/// <summary>
///     MessageViewModel
/// </summary>
public class MessageViewModel
{
    #region Properties

    public string Type { get; set; }
    public string Message { get; set; }

    #endregion Properties
}

#endregion Classes