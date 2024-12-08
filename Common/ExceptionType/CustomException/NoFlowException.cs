using System;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class NoFlowException : Exception
{
    public NoFlowException(MessageId message = MessageId.ThereIsNoFlow) : base(message.GetEnumDescription())
    {
    }
}