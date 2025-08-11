using System.ComponentModel;  
namespace Exportador.UI.Interfaces;

/// <summary>
/// Define um serviço responsável por capturar, exibir e limpar logs em tempo real na interface do usuário.
/// Implementações desta interface devem fornecer notificação de alterações via <see cref="INotifyPropertyChanged"/>.
/// </summary>
public interface ILogViewerService : INotifyPropertyChanged
{
    /// <summary>
    /// Obtém o conteúdo atual do log capturado.
    /// Esse valor deve ser atualizado automaticamente conforme novas entradas de log forem adicionadas.
    /// </summary>
    string CurrentLog { get; }

    /// <summary>
    /// Inicia a captura de logs.
    /// Implementações devem escutar as fontes de log e atualizar <see cref="CurrentLog"/> conforme novas entradas forem recebidas.
    /// </summary>
    void StartCapturing();

    /// <summary>
    /// Interrompe a captura de logs.
    /// Implementações devem parar de escutar as fontes de log após essa chamada.
    /// </summary>
    void StopCapturing();

    /// <summary>
    /// Limpa todo o conteúdo atual do log exibido.
    /// Também deve disparar <see cref="INotifyPropertyChanged.PropertyChanged"/> para a propriedade <see cref="CurrentLog"/>.
    /// </summary>
    void ClearLog();
}
