namespace Exportador.Application.Interfaces;

/// <summary>
/// Interface para serviço de logging.
/// Permite registrar mensagens de log em diferentes níveis: informação, aviso, erro e debug.
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Registra uma mensagem informativa no log.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    void Info(string message);

    /// <summary>
    /// Registra uma mensagem de aviso no log.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    void Warning(string message);

    /// <summary>
    /// Registra uma mensagem de erro no log, com ou sem uma exceção associada.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    /// <param name="ex">Exceção opcional que será registrada com o erro.</param>
    void Error(string message, Exception? ex = null);

    /// <summary>
    /// Registra uma mensagem de erro no log, com uma exceção e uma mensagem descritiva.
    /// </summary>
    /// <param name="ex">A exceção a ser registrada.</param>
    /// <param name="message">Mensagem descritiva adicional para o erro.</param>
    void Error(Exception ex, string message);

    /// <summary>
    /// Registra uma mensagem de depuração no log.
    /// </summary>
    /// <param name="message">Mensagem a ser registrada.</param>
    void Debug(string message);

    /// <summary>
    /// Evento disparado sempre que uma mensagem de log é registrada.
    /// Usado para notificar a UI ou outros componentes sobre novas mensagens de log.
    /// A mensagem passada inclui o nível e o timestamp para exibição.
    /// </summary>
    event Action<string> LogReceived;
}