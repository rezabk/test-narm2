using System;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class NotFoundException : NullReferenceException
{
    public NotFoundException(MessageId messageId = MessageId.EntityDoesNotExist) : base(messageId.GetEnumDescription())
    {
    }
}