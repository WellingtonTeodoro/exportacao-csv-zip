using Exportador.Application.Interfaces;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Serviço que gerencia o estado de conexão e o status da conclusão da exportação.
/// Fornece eventos para notificar mudanças de estado e registra logs via <see cref="ILogService"/>.
/// </summary>
public class ConnectionStateService : IConnectionStateService
{
    private bool _isConnected;
    private bool _isExportCompletedSuccessfully;
    private readonly ILogService _logService;

    /// <summary>
    /// Obtém o estado atual da conexão.
    /// </summary>
    public bool IsConnected
    {
        get => _isConnected;
        private set
        {
            if (_isConnected != value)
            {
                _isConnected = value;
                ConnectionStatusChanged?.Invoke(_isConnected);
                _logService.Debug($"ConnectionStatusChanged para: {_isConnected}");
            }
        }
    }

    /// <summary>
    /// Evento disparado quando o estado de conexão muda.
    /// O valor booleano indica se a conexão está ativa (true) ou inativa (false).
    /// </summary>
    public event Action<bool>? ConnectionStatusChanged;

    /// <summary>
    /// Evento disparado quando o status de conclusão da exportação muda.
    /// O valor booleano indica se a exportação foi concluída com sucesso (true) ou não (false).
    /// </summary>
    public event Action<bool>? ExportCompletedStatusChanged;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="ConnectionStateService"/>.
    /// </summary>
    /// <param name="logService">Serviço de log utilizado para registrar eventos e informações de debug.</param>
    public ConnectionStateService(ILogService logService)
    {
        _logService = logService;
        _isConnected = false;
        _isExportCompletedSuccessfully = false;  
        _logService.Info("ConnectionStateService inicializado.");
    }

    /// <summary>
    /// Define o estado atual da conexão.
    /// </summary>
    /// <param name="isConnected">True para conectado, False para desconectado.</param>
    public void SetConnectionStatus(bool isConnected)
    {
        IsConnected = isConnected;
    }

    /// <summary>
    /// Define o status de conclusão da exportação.
    /// </summary>
    /// <param name="isCompletedSuccessfully">True se a exportação foi concluída com sucesso, False caso contrário.</param>
    public void SetExportCompletedStatus(bool isCompletedSuccessfully)
    {
        if (_isExportCompletedSuccessfully != isCompletedSuccessfully)
        {
            _isExportCompletedSuccessfully = isCompletedSuccessfully;
            ExportCompletedStatusChanged?.Invoke(_isExportCompletedSuccessfully);
            _logService.Debug($"ExportCompletedStatusChanged para: {_isExportCompletedSuccessfully}");
        }
    }

    /// <inheritdoc />
    public bool GetExportCompletedStatus()
    {
        return _isExportCompletedSuccessfully;
    }
}
