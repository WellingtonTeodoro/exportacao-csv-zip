namespace Exportador.Application.Interfaces;

/// <summary>
/// Representa o estado atual do progresso da exportação, contendo uma porcentagem e uma mensagem descritiva.
/// </summary>
public class ExportStatus
{
    /// <summary>
    /// Obtém o valor do progresso em porcentagem (0 a 100).
    /// </summary>
    public int ProgressPercentage { get; }

    /// <summary>
    /// Obtém a mensagem de status atual.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ExportStatus"/>.
    /// </summary>
    /// <param name="progressPercentage">O valor do progresso em porcentagem.</param>
    /// <param name="message">A mensagem de status.</param>
    public ExportStatus(int progressPercentage, string message)
    {
        ProgressPercentage = progressPercentage;
        Message = message;
    }
}

/// <summary>
/// Define um contrato para reportar o progresso de uma operação.
/// Isso desacopla a lógica de negócio da implementação específica de notificação da UI (ex: eventos, callbacks),
/// melhorando a testabilidade e a separação de interesses.
/// </summary>
public interface IProgressReporter
{
    /// <summary>
    /// Obtém a última porcentagem de progresso que foi reportada.
    /// </summary>
    int LastPercentage { get; }

    /// <summary>
    /// Reporta uma nova atualização de status. A implementação deve atualizar o valor de LastPercentage.
    /// </summary>
    /// <param name="status">O objeto de status contendo a porcentagem e a mensagem.</param>
    void Report(ExportStatus status);
}