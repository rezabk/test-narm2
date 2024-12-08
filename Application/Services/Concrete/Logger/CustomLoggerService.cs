using Application.Services.Interface.Logger;
using Microsoft.Extensions.Logging;

namespace Application.Services.Concrete.Logger;

public class CustomLoggerService<TService> : ICustomLoggerService<TService>
    where TService : class
{
    private readonly ILogger _logger;

    public CustomLoggerService(ILogger<TService> logger)
    {
        _logger = logger;
    }

    public void LogAddSuccess(string entityName, int id)
    {
        _logger.LogInformation($"{entityName} with id '{id}' Created successfully");
    }

    public void LogUpdateSuccess(string entityName, int id)
    {
        _logger.LogInformation($"'{entityName}' with id '{id}' Updated successfully");
    }

    public void LogRemoveSuccess(string entityName, int id)
    {
        _logger.LogInformation($"'{entityName}' with id '{id}' Deleted successfully");
    }

    public void LogAddError(Exception? exception, string entityName)
    {
        if (exception == null)
        {
            _logger.LogError($"An error occured while Creating '{entityName}'");
        }
        else
        {
            _logger.LogError(exception, $"An error occured while Creating '{entityName}'");
        }
    }

    public void LogUpdateError(Exception? exception, string entityName, int id)
    {
        if (exception == null)
        {
            _logger.LogError($"An error occured while Updating '{entityName}' with id : '{id}'");
        }
        else
        {
            _logger.LogError(exception,
                $"An error occured while Updating '{entityName}' with id : '{id}'");
        }
    }

    public void LogRemoveError(Exception? exception, string entityName, int id)
    {
        if (exception == null)
        {
            _logger.LogError($"An error occured while Deleting '{entityName}' with id : '{id}'");
        }
        else
        {
            _logger.LogError(exception,
                $"An error occured while Deleting '{entityName}' with id : '{id}'");
        }
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return _logger.BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }
}