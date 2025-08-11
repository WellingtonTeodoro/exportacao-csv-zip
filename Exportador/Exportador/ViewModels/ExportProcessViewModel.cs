using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;
using Exportador.Application.UseCases;
using System.Text;

namespace Exportador.UI.ViewModels;

/// <summary>
/// ViewModel responsável por coordenar o processo de exportação e atualizar a UI com progresso e status.
/// </summary>
public partial class ExportProcessViewModel : ObservableObject, IDisposable
{
    private readonly IExportDataUseCase _exportDataUseCase;
    private readonly ILogService _logService;
    private readonly IConnectionStateService _connectionStateService;

    /// <summary>
    /// Armazena todas as mensagens de log de atividade para exibição na UI.
    /// Utiliza StringBuilder para eficiência na concatenação de strings.
    /// </summary>
    private readonly StringBuilder _activityLogBuilder = new StringBuilder();

    /// <summary>
    /// Propriedade observável que expõe o log de atividades formatado para a UI.
    /// </summary>
    [ObservableProperty]
    private string _activityLog = "Nenhuma atividade registrada ainda...";

    /// <summary>
    /// Indica se o log de atividades da UI está em um estado "limpo" e aguardando a primeira nova mensagem.
    /// </summary>
    private bool _isAwaitingFirstLogAfterReset = true;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ExportProcessViewModel"/>.
    /// </summary>
    /// <param name="exportDataUseCase">Caso de uso responsável por executar a exportação.</param>
    /// <param name="logService">Serviço de log para registrar eventos do processo.</param>
    /// <param name="connectionStateService">Serviço para alterar o estado da conexão após exportação.</param>
    public ExportProcessViewModel(
        IExportDataUseCase exportDataUseCase,
        ILogService logService,
        IConnectionStateService connectionStateService)
    {
        _exportDataUseCase = exportDataUseCase ?? throw new ArgumentNullException(nameof(exportDataUseCase));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _connectionStateService = connectionStateService ?? throw new ArgumentNullException(nameof(connectionStateService));

        _exportDataUseCase.ProgressUpdated += OnExportProgressUpdated;
        _exportDataUseCase.StatusMessageUpdated += OnExportStatusMessageUpdated;
        _logService.LogReceived += OnLogReceived;

        _logService.Info("ExportProcessViewModel inicializado."); 
    }

    /// <summary>
    /// Indica se a exportação está em andamento.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartExportCommand))]
    private bool isExporting;

    /// <summary>
    /// Representa o progresso da exportação (0-100).
    /// </summary>
    [ObservableProperty]
    private int exportProgress;

    /// <summary>
    /// Mensagem de status exibida durante a exportação.
    /// </summary>
    [ObservableProperty]
    private string exportStatusMessage = string.Empty;

    /// <summary>
    /// Comando para iniciar o processo de exportação.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanStartExport))]
    private async Task StartExportAsync()
    {
        if (IsExporting) return;

        try
        {
            IsExporting = true; 
            PrepareForNewVisualLogSession();  

            ExportProgress = 0;
            ExportStatusMessage = "Iniciando exportação...";
             
            _logService.Info("Iniciando exportação...");

            Result result = await _exportDataUseCase.ExecuteExportAsync();

            if (result.IsSuccess)
            {
                ExportProgress = 100;
                ExportStatusMessage = "Exportação concluída.";
                _connectionStateService.SetExportCompletedStatus(true);
            }
            else
            {
                _logService.Error($"Erro ao exportar dados: {result.Error}");
                ExportStatusMessage = $"Erro na exportação: {result.Error}. Verifique os logs.";
                _connectionStateService.SetExportCompletedStatus(false);
                ExportProgress = 0;
            }
        }
        catch (Exception ex)
        {
            _logService.Error("Erro inesperado ao iniciar ou gerenciar exportação: " + ex.Message, ex);
            ExportStatusMessage = "Erro inesperado na exportação. Verifique os logs.";
            _connectionStateService.SetExportCompletedStatus(false);
            ExportProgress = 0;
        }
        finally
        {
            IsExporting = false;
        }
    }

    /// <summary>
    /// Define se o comando de exportação pode ser executado.
    /// </summary>
    /// <returns>True se não estiver exportando.</returns>
    private bool CanStartExport() => !IsExporting;

    /// <summary>
    /// Manipula o evento de atualização de progresso vindo do caso de uso.
    /// </summary>
    /// <param name="sender">Origem do evento.</param>
    /// <param name="progress">Progresso percentual (0 a 100).</param>
    private void OnExportProgressUpdated(object? sender, int progress)
    {
        ExportProgress = progress;
    }

    /// <summary>
    /// Manipula o evento de atualização de mensagem de status vindo do caso de uso.
    /// </summary>
    /// <param name="sender">Origem do evento.</param>
    /// <param name="message">Mensagem de status.</param>
    private void OnExportStatusMessageUpdated(object? sender, string message)
    {
        ExportStatusMessage = message;
    }

    /// <summary>
    /// Manipula o evento <see cref="ILogService.LogReceived"/> para atualizar o log de atividades na UI.
    /// Este método agora forçará a limpeza na *primeira* mensagem após a preparação.
    /// </summary>
    /// <param name="message">A mensagem de log formatada recebida.</param>
    private void OnLogReceived(string message)
    { 
        if (_isAwaitingFirstLogAfterReset)
        {
            _activityLogBuilder.Clear();
            ActivityLog = string.Empty;  
            _isAwaitingFirstLogAfterReset = false;  
        }

        if (_activityLogBuilder.Length > 0)
        {
            _activityLogBuilder.AppendLine();
        }
        _activityLogBuilder.Append(message);

        ActivityLog = _activityLogBuilder.ToString();
    }
     
    /// <summary>
    /// Prepara o estado interno do log de atividades para uma nova sessão de exibição,
    /// marcando-o para ser limpo na próxima mensagem recebida.
    /// </summary>
    private void PrepareForNewVisualLogSession()
    {
        _isAwaitingFirstLogAfterReset = true;  
    } 

    /// <summary>
    /// Reseta o estado do ExportProcessViewModel para seu ponto inicial,
    /// limpando o log, progresso e mensagens de status.
    /// </summary>
    [RelayCommand]
    public void Reset()
    {
        _logService.Info("ExportProcessViewModel.Reset() chamado. Limpando estado da exportação.");

        PrepareForNewVisualLogSession();  

        ExportProgress = 0;
        ExportStatusMessage = string.Empty;
        IsExporting = false;
        _connectionStateService.SetExportCompletedStatus(false);

        OnPropertyChanged(nameof(ExportProgress));
        OnPropertyChanged(nameof(ExportStatusMessage));
        OnPropertyChanged(nameof(IsExporting));
        OnPropertyChanged(nameof(ActivityLog));
        StartExportCommand.NotifyCanExecuteChanged();
         
        _logService.Info("Esperando processo de exportação.");
    }

    /// <summary>
    /// Remove os event handlers para evitar vazamentos de memória.
    /// Este método deve ser chamado quando o ViewModel não é mais necessário (ex: fechamento da janela).
    /// </summary>
    public void Dispose()
    {
        _exportDataUseCase.ProgressUpdated -= OnExportProgressUpdated;
        _exportDataUseCase.StatusMessageUpdated -= OnExportStatusMessageUpdated;
        _logService.LogReceived -= OnLogReceived;
        GC.SuppressFinalize(this);
    }
}