using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class ErrorException : Exception
{
    public ErrorException(MessageId messageId = MessageId.Exception, params string[] placeHolders) : base(
        ErrorExceptionHelper.CreateViewMessage(messageId, placeHolders))
    {
    }

    public ErrorException(List<string> messages) : base(string.Join(", ", messages))
    {
    }
}