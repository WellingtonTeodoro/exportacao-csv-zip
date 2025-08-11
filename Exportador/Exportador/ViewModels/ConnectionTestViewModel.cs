using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;
using Exportador.UI.Interfaces;
using Exportador.UI.Services;
#nullable disable

namespace Exportador.UI.ViewModels;

/// <summary>
/// ViewModel responsável pelo teste de conexão ao banco de dados.
/// Gerencia os parâmetros da conexão, execução do teste, e status da conexão.
/// </summary>
public partial class ConnectionTestViewModel : ObservableObject
{
    private readonly ILogService _logService;
    private readonly IConnectionStateService _connectionStateService;
    private readonly IDatabaseConnectionService _databaseConnectionService;
    private readonly IConnectionParametersStore _connectionParametersStore;

    /// <summary>
    /// Provedor para captura segura da senha (não armazenada diretamente neste ViewModel).
    /// </summary>
    public IPasswordProvider PasswordProvider { get; }

    /// <summary>
    /// Nome do servidor de banco de dados para a conexão.
    /// </summary>
    [ObservableProperty]
    private string _serverName = string.Empty;

    /// <summary>
    /// Nome do banco de dados para a conexão.
    /// </summary>
    [ObservableProperty]
    private string _databaseName = string.Empty;

    /// <summary>
    /// Nome do usuário para a conexão.
    /// </summary>
    [ObservableProperty]
    private string _userName = string.Empty;

    /// <summary>
    /// Indica se o teste de conexão está em andamento, para evitar testes simultâneos.
    /// </summary>
    [ObservableProperty]
    private bool _isTestingConnection;

    /// <summary>
    /// Resultado atual do teste de conexão, incluindo sucesso ou mensagem de erro.
    /// </summary>
    [ObservableProperty]
    private ConnectionTestResult _connectionTestResult = ConnectionTestResult.Fail("Nenhum teste de conexão realizado.");

    /// <summary>
    /// Mensagem para exibir o status atual da conexão na interface.
    /// </summary>
    [ObservableProperty]
    private string _connectionStatusMessage;

    /// <summary>
    /// Comando acionado para executar o teste de conexão.
    /// </summary>
    public IRelayCommand TestConnectionCommand { get; }

    /// <summary>
    /// Inicializa uma nova instância do ViewModel, injetando os serviços necessários.
    /// </summary>
    /// <param name="logService">Serviço para registrar logs.</param>
    /// <param name="connectionStateService">Serviço para gerenciar estado da conexão.</param>
    /// <param name="databaseConnectionService">Serviço para testar conexão ao banco.</param>
    /// <param name="connectionParametersStore">Armazenamento dos parâmetros de conexão persistidos.</param>
    /// <exception cref="ArgumentNullException">Se algum parâmetro for nulo.</exception>
    public ConnectionTestViewModel(
        ILogService logService,
        IConnectionStateService connectionStateService,
        IDatabaseConnectionService databaseConnectionService,
        IConnectionParametersStore connectionParametersStore)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _connectionStateService = connectionStateService ?? throw new ArgumentNullException(nameof(connectionStateService));
        _databaseConnectionService = databaseConnectionService ?? throw new ArgumentNullException(nameof(databaseConnectionService));
        _connectionParametersStore = connectionParametersStore ?? throw new ArgumentNullException(nameof(connectionParametersStore));

        PasswordProvider = new PasswordProvider();

        ConnectionStatusMessage = "Aguardando teste de conexão...";
        _connectionStateService.SetConnectionStatus(false);
        _logService.Info("ConnectionTestViewModel inicializado.");

        TestConnectionCommand = new RelayCommand(
            async () => await TestConnectionAsyncInternal(),
            CanTestConnection);
         
        PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ServerName) ||
                e.PropertyName == nameof(DatabaseName) ||
                e.PropertyName == nameof(UserName) ||
                e.PropertyName == nameof(IsTestingConnection))
            {
                TestConnectionCommand.NotifyCanExecuteChanged();
            }
        };
         
        if (PasswordProvider is ObservableObject observablePasswordProvider)
        {
            observablePasswordProvider.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PasswordProvider.Password))
                {
                    TestConnectionCommand.NotifyCanExecuteChanged();
                }
            };
        }

        LoadSavedConnectionParameters();
    }

    /// <summary>
    /// Determina se o comando de teste de conexão pode ser executado,
    /// verificando se todos os parâmetros são válidos e se não há teste em andamento.
    /// </summary>
    /// <returns>True se pode executar, false caso contrário.</returns>
    private bool CanTestConnection()
    {
        return !IsTestingConnection &&
               !string.IsNullOrWhiteSpace(ServerName) &&
               !string.IsNullOrWhiteSpace(DatabaseName) &&
               !string.IsNullOrWhiteSpace(UserName) &&
               PasswordProvider.Password != null &&
               PasswordProvider.Password.Length > 0;
    }

    /// <summary>
    /// Executa o teste de conexão de forma assíncrona,
    /// atualizando o estado, mensagens, armazenando parâmetros e lidando com erros.
    /// </summary>
    private async Task TestConnectionAsyncInternal()
    {
        Reset();

        if (IsTestingConnection) return;

        IsTestingConnection = true;
        _connectionStateService.SetConnectionStatus(false);
        ConnectionStatusMessage = "Testando conexão...";
        ConnectionTestResult = ConnectionTestResult.Fail("Testando...");
        _logService.Info("Iniciando teste de conexão (via comando).");

        var parametersCriados = ConnectionParameters.Create(
            ServerName,
            DatabaseName,
            UserName,
            PasswordProvider.Password
        );

        if (parametersCriados.IsFailure)
        {
            ConnectionStatusMessage = parametersCriados.Error;
            ConnectionTestResult = ConnectionTestResult.Fail(parametersCriados.Error);
            _connectionStateService.SetConnectionStatus(false);
            _connectionParametersStore.CurrentParameters = null;
            _logService.Warning($"Parâmetros de conexão inválidos: {parametersCriados.Error}");
            IsTestingConnection = false;
            return;
        }

        var parameters = parametersCriados.Value;

        try
        {
            ConnectionTestResult testResult = await _databaseConnectionService.TestConnectionAsync(parameters);

            ConnectionTestResult = testResult;

            if (testResult.IsSuccess)
            {
                ConnectionStatusMessage = "Conexão bem-sucedida!";
                _connectionStateService.SetConnectionStatus(true);
                _connectionParametersStore.CurrentParameters = parameters;
                _logService.Info("Teste de conexão: SUCESSO. Parâmetros armazenados.");
            }
            else
            {
                ConnectionStatusMessage = testResult.ErrorMessage;
                _connectionStateService.SetConnectionStatus(false);
                _connectionParametersStore.CurrentParameters = null;
                _logService.Warning($"Teste de conexão: FALHA. {testResult.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            ConnectionStatusMessage = $"Erro inesperado: {ex.Message}";
            ConnectionTestResult = ConnectionTestResult.Fail($"Erro inesperado: {ex.Message}");
            _connectionStateService.SetConnectionStatus(false);
            _connectionParametersStore.CurrentParameters = null;
            _logService.Error($"Erro grave durante o teste de conexão: {ex.Message}", ex);
        }
        finally
        {
            IsTestingConnection = false;
        }
    }

    /// <summary>
    /// Carrega parâmetros de conexão previamente salvos do armazenamento,
    /// atualizando os campos e estado da conexão conforme necessário.
    /// </summary>
    private void LoadSavedConnectionParameters()
    {
        if (_connectionParametersStore.HasParameters && _connectionParametersStore.CurrentParameters != null)
        {
            ServerName = _connectionParametersStore.CurrentParameters.Server;
            DatabaseName = _connectionParametersStore.CurrentParameters.Database;
            UserName = _connectionParametersStore.CurrentParameters.User;
            PasswordProvider.Password = _connectionParametersStore.CurrentParameters.Password;

            _logService.Debug("Parâmetros de conexão carregados do armazenamento.");

            ConnectionTestResult = _connectionStateService.IsConnected
                ? ConnectionTestResult.Success()
                : ConnectionTestResult.Fail("Parâmetros carregados. Status de conexão desconhecido ou falho.");

            ConnectionStatusMessage = ConnectionTestResult.IsSuccess
                ? "Conexão pré-configurada OK."
                : ConnectionTestResult.ErrorMessage;
        }
        else
        {
            _logService.Debug("Nenhum parâmetro de conexão salvo encontrado.");
        }
        TestConnectionCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Reseta o estado do teste de conexão, atualizando a mensagem de status para o estado inicial.
    /// </summary>
    public void Reset()
    { 
        ConnectionStatusMessage = "Aguardando teste de conexão..."; 
        ConnectionTestResult = ConnectionTestResult.Fail("Nenhum teste de conexão realizado.");
         
        _connectionStateService.SetConnectionStatus(false);
         
        IsTestingConnection = false;  
        TestConnectionCommand.NotifyCanExecuteChanged();

        _logService.Debug("ConnectionTestViewModel resetado para o estado inicial.");
    }
}
