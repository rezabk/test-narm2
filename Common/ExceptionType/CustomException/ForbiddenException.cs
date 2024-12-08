using System;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class ForbiddenException : Exception
{
    public ForbiddenException(MessageId messageId = MessageId.AccessDenied) : base(messageId.GetEnumDescription())
    {
    }
}