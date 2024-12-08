using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class CompleteProfileException : ValidationException
{
    public CompleteProfileException(MessageId messageId) : base(messageId.GetEnumDescription())
    {
    }


    public CompleteProfileException(MessageId messageId, object specificObject)
        : base(messageId.GetEnumDescription())
    {
        SpecificObject = specificObject;
    }

    public object SpecificObject { get; }
}