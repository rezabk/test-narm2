using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class ChangeStatusException : ValidationException
{
    public ChangeStatusException(MessageId messageId) : base(messageId.GetEnumDescription())
    {
    }


    public ChangeStatusException(MessageId messageId, object specificObject)
        : base(messageId.GetEnumDescription())
    {
        SpecificObject = specificObject;
    }

    public object SpecificObject { get; }
}