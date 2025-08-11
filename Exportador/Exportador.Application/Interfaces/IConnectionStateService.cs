namespace Exportador.Application.Interfaces;

/// <summary>
/// Serviço responsável por gerenciar e fornecer o estado atual da conexão e da exportação.
/// Permite notificar assinantes sobre mudanças nesses estados via eventos.
/// </summary>
public interface IConnectionStateService
{
    /// <summary>
    /// Indica se a aplicação está atualmente conectada ao banco de dados.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Evento disparado sempre que o status de conexão muda.
    /// O valor booleano indica o novo estado: true = conectado, false = desconectado.
    /// </summary>
    event Action<bool> ConnectionStatusChanged;

    /// <summary>
    /// Evento disparado quando o status de conclusão da exportação muda.
    /// O valor booleano indica se a exportação foi concluída com sucesso.
    /// </summary>
    event Action<bool> ExportCompletedStatusChanged;

    /// <summary>
    /// Define o estado atual da conexão.
    /// Dispara o evento <see cref="ConnectionStatusChanged"/> se houver alteração.
    /// </summary>
    /// <param name="isConnected">True se conectado, False caso contrário.</param>
    void SetConnectionStatus(bool isConnected);

    /// <summary>
    /// Define o status de conclusão da exportação.
    /// Dispara o evento <see cref="ExportCompletedStatusChanged"/> se houver alteração.
    /// </summary>
    /// <param name="isCompletedSuccessfully">True se a exportação foi concluída com sucesso, False se falhou ou está em andamento.</param>
    void SetExportCompletedStatus(bool isCompletedSuccessfully);

    /// <summary>
    /// Obtém o status de conclusão da exportação.
    /// </summary>
    /// <returns>True se a exportação foi concluída com sucesso, False caso contrário (incluindo em andamento ou falha).</returns>
    bool GetExportCompletedStatus();
}
