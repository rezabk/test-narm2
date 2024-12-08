using Microsoft.Extensions.Logging;

namespace Application.Services.Interface.Logger;

public interface ICustomLoggerService<TService> : ILogger<TService>
    where TService : class
{
    void LogAddSuccess(string entityName, int id);
    void LogUpdateSuccess(string entityName, int id);
    void LogRemoveSuccess(string entityName, int id);

    void LogAddError(Exception? exception, string entityName);

    void LogUpdateError(Exception? exception, string entityName, int id);

    void LogRemoveError(Exception? exception, string entityName, int id);
}