using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Implementação concreta de <see cref="IConnectionParametersStore"/> que armazena os parâmetros de conexão
/// em memória durante o ciclo de vida da aplicação.
/// Esta classe é projetada para ser um singleton e gerenciar os parâmetros de conexão globais da aplicação.
/// </summary>
public class ConnectionParametersStore : IConnectionParametersStore
{
    private ConnectionParameters? _currentParameters;
    private readonly ILogService _logService;  

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ConnectionParametersStore"/>.
    /// </summary>
    /// <param name="logService">O serviço de log para registrar eventos e informações.</param>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="logService"/> for nulo.</exception>
    public ConnectionParametersStore(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService), "O serviço de log não pode ser nulo."); 
        _logService.Info("ConnectionParametersStore inicializado.");
    }

    /// <summary>
    /// Obtém ou define os parâmetros de conexão atuais.
    /// Quando um novo valor é atribuído, o armazenamento interno é atualizado
    /// e um log é registrado.
    /// </summary>
    public ConnectionParameters CurrentParameters
    {
        get
        { 
            _logService.Info("Parâmetros de conexão sendo acessados.");
            return _currentParameters!;
        }
        set
        { 
            if (_currentParameters != value)
            {
                _currentParameters = value; 
                _logService.Info("Parâmetros de conexão atualizados.");
            }
        }
    }

    /// <summary>
    /// Indica se os parâmetros de conexão já foram definidos nesta instância do armazenamento.
    /// </summary>
    public bool HasParameters => _currentParameters != null;
}