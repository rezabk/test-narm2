using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.ExceptionType.CustomException;

public class FormValidationException : ValidationException
{
    public FormValidationException(MessageId messageId) : base(messageId.GetEnumDescription())
    {
    }


    public FormValidationException(MessageId messageId, object specificObject)
        : base(messageId.GetEnumDescription())
    {
        SpecificObject = specificObject;
    }

    public FormValidationException(MessageId messageId, string errorMessage)
        : base(string.Format(messageId.GetEnumDescription(), errorMessage))
    {
    }

    public FormValidationException(MessageId messageId, List<string> errorMessages)
        : base(messageId.GetEnumDescription())
    {
        HelperMessages = errorMessages ?? new List<string>();
    }

    public object SpecificObject { get; }
    public List<string> HelperMessages { get; }
}