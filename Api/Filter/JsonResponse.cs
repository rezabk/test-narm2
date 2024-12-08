namespace Api.Filter;

public record ErrorJsonResponse(string[] Messages, object? HelperMessage);

public record SuccessJsonResponse(object result, string message);

public record ValidationFailedJsonResponse(object value, string message);