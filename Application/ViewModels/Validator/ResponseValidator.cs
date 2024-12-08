using Common.ExceptionType.CustomException;

namespace Application.ViewModels.Validator;

public class ResponseValidator
{
    public bool Success { get; set; }

    public FormValidationException Exception { get; set; }
}