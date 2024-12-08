using System;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class AccountIsLockedException : Exception
{
    public AccountIsLockedException(MessageId messageId = MessageId.AccountLocked) : base(
        messageId.GetEnumDescription())
    {
    }
}