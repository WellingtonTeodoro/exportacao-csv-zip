using Exportador.Application.Interfaces;
using Serilog;

namespace Exportador.Infrastructure.Logging;

/// <summary>
/// Implementação de <see cref="ILogService"/> usando Serilog como backend de logging.
/// Os logs são gravados em arquivos com rotação diária no diretório 'logs'.
/// </summary>
public class LogService : ILogService
{
    /// <summary>
    /// Evento disparado sempre que uma mensagem de log é registrada.
    /// </summary>
    public event Action<string>? LogReceived;

    /// <summary>
    /// Registra uma mensagem informativa no log.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    public void Info(string message)
    {
        var formattedMessage = $"[INFO] {DateTime.Now:HH:mm:ss} - {message}";
        Log.Information(message);
        LogReceived?.Invoke(formattedMessage);  
    }

    /// <summary>
    /// Registra uma mensagem de aviso no log.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    public void Warning(string message)
    {
        var formattedMessage = $"[WARN] {DateTime.Now:HH:mm:ss} - {message}";
        Log.Warning(message);
        LogReceived?.Invoke(formattedMessage);  
    }

    /// <summary>
    /// Registra uma mensagem de erro no log, com ou sem uma exceção associada.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    /// <param name="ex">Exceção opcional que será registrada com o erro.</param>
    public void Error(string message, Exception? ex = null)
    {
        var formattedMessage = $"[ERROR] {DateTime.Now:HH:mm:ss} - {message}";
        Log.Error(ex, message);
        LogReceived?.Invoke(formattedMessage);  
    }

    /// <summary>
    /// Registra uma mensagem de depuração no log.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    public void Debug(string message)
    {
        var formattedMessage = $"[DEBUG] {DateTime.Now:HH:mm:ss} - {message}";
        Log.Debug(message);
        LogReceived?.Invoke(formattedMessage);  
    }

    /// <summary>
    /// Registra uma mensagem de erro no log, com uma exceção e uma mensagem descritiva.
    /// </summary>
    /// <param name="ex">A exceção a ser registrada.</param>
    /// <param name="message">Mensagem descritiva adicional para o erro.</param>
    public void Error(Exception ex, string message)
    {
        Error(message, ex);
    }
}