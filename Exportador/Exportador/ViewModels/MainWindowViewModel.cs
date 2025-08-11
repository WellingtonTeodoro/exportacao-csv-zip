using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exportador.Application.Interfaces;
using Exportador.UI.Views;
using System.ComponentModel;
using System.Windows;

namespace Exportador.UI.ViewModels;

/// <summary>
/// ViewModel principal da MainWindow, responsável por controlar o fluxo do wizard,
/// a troca das Views conforme etapas, e o estado dos comandos de navegação.
/// Implementa IDisposable para gerenciar eventos e evitar memory leaks.
/// </summary>
public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    private readonly ILogService _logService;
    private readonly IConnectionStateService _connectionStateService;

    private readonly ConnectionTestViewModel _connectionTestViewModel;
    private readonly EntitySelectionViewModel _entitySelectionViewModel;
    private readonly DestinationDirectoryViewModel _destinationDirectoryViewModel;
    private readonly ExportProcessViewModel _exportProcessViewModel;
    private readonly ResultViewModel _resultViewModel;

    /// <summary>
    /// Índice da etapa atual no wizard (0 a 4).
    /// Controla qual View será exibida.
    /// </summary>
    [ObservableProperty]
    private int _currentStepIndex;

    /// <summary>
    /// View atualmente exibida na MainWindow,
    /// sincronizada automaticamente com o CurrentStepIndex.
    /// </summary>
    [ObservableProperty] 
    private System.Windows.Controls.UserControl _currentView = new System.Windows.Controls.UserControl();

    /// <summary>
    /// Controla a visibilidade do botão 'Próximo'.
    /// Será Collapsed na ResultView.
    /// </summary>
    [ObservableProperty]
    private Visibility _isNextButtonVisible;

    /// <summary>
    /// Controla a visibilidade do botão 'Anterior'.
    /// Será Collapsed na ResultView.
    /// </summary>
    [ObservableProperty]
    private Visibility _isPreviousButtonVisible;

    /// <summary>
    /// Controla a visibilidade do botão 'Finalizar'.
    /// Será Collapsed na ResultView.
    /// </summary>
    [ObservableProperty]
    private Visibility _isFinishButtonVisible;

    /// <summary>
    /// Controla a visibilidade do botão 'Nova Exportação'.
    /// Será Collapsed na ResultView.
    /// </summary>
    [ObservableProperty]
    private Visibility _isNovaButtonVisible;

    /// <summary>
    /// Controla a visibilidade do botão 'Fechar'.
    /// Será Collapsed na ResultView.
    /// </summary>
    [ObservableProperty]
    private Visibility _isCloseButtonVisible;

    /// <summary>
    /// Controla a visibilidade do botão 'Cancelar'.
    /// Será Collapsed na ResultView.
    /// </summary>
    [ObservableProperty]
    private Visibility _isCancelButtonVisible;
     
    private PropertyChangedEventHandler? _connectionTestViewModelPropertyChangedHandler;
    private PropertyChangedEventHandler? _entitySelectionViewModelPropertyChangedHandler;
    private PropertyChangedEventHandler? _destinationDirectoryViewModelPropertyChangedHandler;
    private PropertyChangedEventHandler? _exportProcessViewModelPropertyChangedHandler;

    /// <summary>
    /// Construtor da MainWindowViewModel.
    /// Injeta dependências, assina eventos, inicializa estado.
    /// </summary>
    public MainWindowViewModel(
        ILogService logService,
        IConnectionStateService connectionStateService,
        ConnectionTestViewModel connectionTestViewModel,
        EntitySelectionViewModel entitySelectionViewModel,
        DestinationDirectoryViewModel destinationDirectoryViewModel,
        ExportProcessViewModel exportProcessViewModel,
        ResultViewModel resultViewModel)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _connectionStateService = connectionStateService ?? throw new ArgumentNullException(nameof(connectionStateService));
        _connectionTestViewModel = connectionTestViewModel ?? throw new ArgumentNullException(nameof(connectionTestViewModel));
        _entitySelectionViewModel = entitySelectionViewModel ?? throw new ArgumentNullException(nameof(entitySelectionViewModel));
        _destinationDirectoryViewModel = destinationDirectoryViewModel ?? throw new ArgumentNullException(nameof(destinationDirectoryViewModel));
        _exportProcessViewModel = exportProcessViewModel ?? throw new ArgumentNullException(nameof(exportProcessViewModel));
        _resultViewModel = resultViewModel ?? throw new ArgumentNullException(nameof(resultViewModel));
         
        _connectionStateService.ConnectionStatusChanged += OnConnectionStatusChanged;
        _connectionStateService.ExportCompletedStatusChanged += OnExportCompletedStatusChanged;
         
        _connectionTestViewModelPropertyChangedHandler = (s, e) =>
        {
            if (e.PropertyName == nameof(_connectionTestViewModel.ConnectionTestResult) ||
                e.PropertyName == nameof(_connectionTestViewModel.IsTestingConnection))
            {
                NavigateNextCommand.NotifyCanExecuteChanged();
                _logService.Debug($"MainWindowViewModel reagiu à mudança em ConnectionTestViewModel: {e.PropertyName}");
            }
        };
        _connectionTestViewModel.PropertyChanged += _connectionTestViewModelPropertyChangedHandler;

        _entitySelectionViewModelPropertyChangedHandler = (s, e) =>
        {
            if (e.PropertyName == nameof(_entitySelectionViewModel.CanProceed))
            {
                NavigateNextCommand.NotifyCanExecuteChanged();
                _logService.Debug($"MainWindowViewModel reagiu à mudança em EntitySelectionViewModel: {e.PropertyName}");
            }
        };
        _entitySelectionViewModel.PropertyChanged += _entitySelectionViewModelPropertyChangedHandler;

        _destinationDirectoryViewModelPropertyChangedHandler = (s, e) =>
        {
            if (e.PropertyName == nameof(_destinationDirectoryViewModel.CanProceed))
            {
                NavigateNextCommand.NotifyCanExecuteChanged();
                FinishCommand.NotifyCanExecuteChanged();
                _logService.Debug($"MainWindowViewModel reagiu à mudança em DestinationDirectoryViewModel: {e.PropertyName}");
            }
        };
        _destinationDirectoryViewModel.PropertyChanged += _destinationDirectoryViewModelPropertyChangedHandler;

        _exportProcessViewModelPropertyChangedHandler = (s, e) =>
        {
            if (e.PropertyName == nameof(_exportProcessViewModel.IsExporting))
            {
                NavigateNextCommand.NotifyCanExecuteChanged();
                FinishCommand.NotifyCanExecuteChanged(); 
            }
            if (e.PropertyName == nameof(_exportProcessViewModel.ExportProgress) && _exportProcessViewModel.ExportProgress == 100)
            {
                NavigateNextCommand.NotifyCanExecuteChanged();
                FinishCommand.NotifyCanExecuteChanged();
                _logService.Debug("ExportProcessViewModel.ExportProgress atingiu 100%. Reavaliando comandos.");
            }
        };
        _exportProcessViewModel.PropertyChanged += _exportProcessViewModelPropertyChangedHandler;

        _destinationDirectoryViewModel.OutputFileName = "novo_arquivo_export";
        CurrentStepIndex = 0;
        UpdateCurrentView();
        _logService.Info("MainWindowViewModel inicializado.");
        UpdateCommandStates();
        UpdateAllButtonVisibilities();
    }

    /// <summary>
    /// Evento chamado quando o estado da conexão muda.
    /// Reavalia comandos dependentes.
    /// </summary>
    private void OnConnectionStatusChanged(bool isConnected)
    {
        NavigateNextCommand.NotifyCanExecuteChanged();
        _logService.Debug($"Status de conexão alterado para: {isConnected}");
    }

    /// <summary>
    /// Evento chamado quando o status da exportação é atualizado.
    /// Reavalia comandos dependentes.
    /// </summary>
    private void OnExportCompletedStatusChanged(bool isCompletedSuccessfully)
    {
        _logService.Info($"Status de conclusão da exportação: {isCompletedSuccessfully}");
        NavigateNextCommand.NotifyCanExecuteChanged();
        FinishCommand.NotifyCanExecuteChanged();

        UpdateCommandStates();
        UpdateAllButtonVisibilities();
    }

    /// <summary>
    /// Comando para navegar para a etapa anterior do wizard.
    /// Este comando é habilitado se <see cref="CanNavigatePrevious"/> retornar <see langword="true"/>.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigatePrevious))]
    private void NavigatePrevious()
    {
        if (CurrentStepIndex > 0)
        {
            CurrentStepIndex--;
            UpdateCurrentView();
            _logService.Info($"Navegou para etapa anterior: {CurrentStepIndex}");
        }
        UpdateCommandStates();
        UpdateAllButtonVisibilities();
    }

    /// <summary>
    /// Comando para navegar para a próxima etapa do wizard.
    /// Este comando é habilitado se <see cref="CanNavigateNext"/> retornar <see langword="true"/>.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateNext))]
    private void NavigateNext()
    {
        if (CanNavigateNext())
        {
            int nextStep = CurrentStepIndex + 1;

            CurrentStepIndex = nextStep;
            UpdateCurrentView();
            _logService.Info($"Navegou para próxima etapa: {CurrentStepIndex}");
        }
        UpdateCommandStates();
        UpdateAllButtonVisibilities();
        if (CurrentStepIndex == GetExportProcessStepIndex())
        {
            _exportProcessViewModel.Reset();
        }
    }

    /// <summary>
    /// Comando para finalizar o processo do wizard ou navegar para a tela de resultados.
    /// Este comando é habilitado se <see cref="CanFinish"/> retornar <see langword="true"/>.
    /// </summary>
    /// <remarks>
    /// Se estiver na etapa de exportação e a exportação estiver concluída com sucesso, navega para a tela de resultados.
    /// Caso contrário, se já estiver na tela de resultados, registra uma mensagem de finalização.
    /// </remarks>
    [RelayCommand(CanExecute = nameof(CanFinish))]
    private void Finish()
    {
        _logService.Info("Comando Finalizar executado.");

        if (CurrentStepIndex == GetExportProcessStepIndex() && _connectionStateService.GetExportCompletedStatus())
        {
            CurrentStepIndex = GetResultStepIndex();
            UpdateCurrentView();
            _logService.Info("Navegou para etapa Resultado após exportação.");
        }
        else if (CurrentStepIndex == GetResultStepIndex())
        {
            _logService.Info("Finalizando processo do wizard.");
        }
        UpdateCommandStates();
        UpdateAllButtonVisibilities();
    }
     
    /// <summary>
    /// Determina se o comando <see cref="NavigatePreviousCommand"/> pode ser executado.
    /// O comando é habilitado se o índice da etapa atual for maior que 0,
    /// a exportação não estiver em andamento e não estiver na etapa de resultados.
    /// </summary>
    /// <returns><see langword="true"/> se o comando puder ser executado; caso contrário, <see langword="false"/>.</returns>
    private bool CanNavigatePrevious()
    {
        bool isExporting = CurrentStepIndex == GetExportProcessStepIndex() && _exportProcessViewModel.IsExporting;
        bool isAtResultStep = CurrentStepIndex == GetResultStepIndex();

        return CurrentStepIndex > 0 && !isExporting && !isAtResultStep;
    }

    /// <summary>
    /// Determina se o comando <see cref="NavigateNextCommand"/> pode ser executado.
    /// A habilitação depende da validação específica de cada ViewModel para a etapa atual.
    /// </summary>
    /// <returns><see langword="true"/> se o comando puder ser executado; caso contrário, <see langword="false"/>.</returns>
    private bool CanNavigateNext()
    {
        return CurrentStepIndex switch
        {
            0 => _connectionStateService.IsConnected && !_connectionTestViewModel.IsTestingConnection,
            1 => _entitySelectionViewModel.CanProceed,
            2 => _destinationDirectoryViewModel.CanProceed,
            3 => false,  
            4 => false,  
            _ => false,
        };
    }

    /// <summary>
    /// Determina se o comando <see cref="FinishCommand"/> pode ser executado.
    /// O comando é habilitado se a exportação foi concluída com sucesso na etapa de exportação,
    /// ou se já estiver na etapa de resultados.
    /// </summary>
    /// <returns><see langword="true"/> se o comando puder ser executado; caso contrário, <see langword="false"/>.</returns>
    private bool CanFinish()
    {
        bool atExportStepCompleted = CurrentStepIndex == GetExportProcessStepIndex() && _connectionStateService.GetExportCompletedStatus();
        bool atResultStep = CurrentStepIndex == GetResultStepIndex();
        return atExportStepCompleted || atResultStep;
    }

    /// <summary>
    /// Comando para cancelar a operação e fechar o aplicativo.
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        _logService.Info("Comando Cancelar executado. Encerrando a aplicação.");
        App.Current.Shutdown();
    }

    /// <summary>
    /// Comando para cancelar a operação e fechar o aplicativo.
    /// </summary>
    [RelayCommand]
    private void Close()
    {
        _logService.Info("Comando Fechar executado. Encerrando a aplicação.");
        App.Current.Shutdown();
    }

    /// <summary>
    /// Comando para iniciar uma nova exportação.
    /// </summary>
    [RelayCommand]
    private void Nova()
    {
        _logService.Info("Comando 'Nova Exportação' executado. Reiniciando o wizard.");

         _connectionTestViewModel.Reset();         
        _entitySelectionViewModel.Reset();         
        _exportProcessViewModel.Reset();          
        _resultViewModel.Reset();
        _destinationDirectoryViewModel.OutputFileName = "novo_arquivo_export";
        _connectionStateService.SetExportCompletedStatus(false);  
         
        CurrentStepIndex = 0;
        UpdateCurrentView();  
         
        UpdateCommandStates();
        UpdateAllButtonVisibilities();

        _logService.Info("Aplicativo resetado para o estado inicial (exceto dados de conexão).");
    }

    /// <summary>
    /// Atualiza os estados dos comandos de navegação.
    /// </summary>
    private void UpdateCommandStates()
    {
        NavigatePreviousCommand.NotifyCanExecuteChanged();
        NavigateNextCommand.NotifyCanExecuteChanged();
        FinishCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
        CloseCommand.NotifyCanExecuteChanged();
        NovaCommand.NotifyCanExecuteChanged();
    }

    private int GetTotalSteps() => 5;  

    private int GetExportProcessStepIndex() => 3;  

    private int GetResultStepIndex() => 4;  

    /// <summary>
    /// Atualiza a View atual baseada no CurrentStepIndex.
    /// </summary>
    private void UpdateCurrentView()
    {
        CurrentView = CurrentStepIndex switch
        {
            0 => new ConnectionTestView { DataContext = _connectionTestViewModel },
            1 => new EntitySelectionView { DataContext = _entitySelectionViewModel },
            2 => new DestinationDirectoryView { DataContext = _destinationDirectoryViewModel },
            3 => new ExportProcessView { DataContext = _exportProcessViewModel },
            4 => new ResultView { DataContext = _resultViewModel },
            _ => new System.Windows.Controls.UserControl()
        };

        _logService.Info($"Atualizou CurrentView para etapa {CurrentStepIndex}");
    }

    /// <summary>
    /// Define a visibilidade do botão "Próximo" com base na etapa atual.
    /// </summary>
    private void UpdateNextButtonVisibility()  
    {
        IsNextButtonVisible = (CurrentStepIndex == GetResultStepIndex() || CurrentStepIndex == GetExportProcessStepIndex()) 
            ? Visibility.Collapsed : Visibility.Visible;

        _logService.Debug($"Visibilidade do botão Próximo atualizada para: {IsNextButtonVisible}");
    }

    /// <summary>
    /// Define a visibilidade do botão "Anterior" com base na etapa atual.
    /// </summary>
    private void UpdatePreviousButtonVisible()
    {
        IsPreviousButtonVisible = (CurrentStepIndex == 4 || (CurrentStepIndex == GetExportProcessStepIndex() 
            && _connectionStateService.GetExportCompletedStatus()) || CurrentStepIndex == 0)
            ? Visibility.Collapsed : Visibility.Visible;

        _logService.Debug($"Visibilidade do botão Anterior atualizada para: {IsPreviousButtonVisible}");
    }

    /// <summary>
    /// Define a visibilidade do botão "Finalizar" com base na etapa atual.
    /// </summary>
    private void UpdateFinishButtonVisible()
    {
        IsFinishButtonVisible = (CurrentStepIndex == GetExportProcessStepIndex() && _connectionStateService.GetExportCompletedStatus())
            ? Visibility.Visible : Visibility.Collapsed;

        _logService.Debug($"Visibilidade do botão Finalizar atualizada para: {IsFinishButtonVisible}");
    }

    /// <summary>
    /// Define a visibilidade do botão "Nova Exportação" com base na etapa atual.
    /// </summary>
    private void UpdateNovaButtonVisible()
    {
        IsNovaButtonVisible = (CurrentStepIndex == GetResultStepIndex()) 
            ? Visibility.Visible : Visibility.Collapsed;

        _logService.Debug($"Visibilidade do botão Nova Exportação atualizada para: {IsNovaButtonVisible}");
    }

    /// <summary>
    /// Define a visibilidade do botão "Fechar" com base na etapa atual.
    /// </summary>
    private void UpdateCloseButtonVisible()
    {
        IsCloseButtonVisible = (CurrentStepIndex == GetResultStepIndex()) 
            ? Visibility.Visible : Visibility.Collapsed;

        _logService.Debug($"Visibilidade do botão Fechar atualizada para: {IsCloseButtonVisible}");
    }
    
    /// <summary>
    /// Define a visibilidade do botão "Cancelar" com base na etapa atual.
    /// </summary>
    private void UpdateCancelButtonVisible()
    {
        IsCancelButtonVisible = (CurrentStepIndex == GetResultStepIndex() ||
            (CurrentStepIndex == GetExportProcessStepIndex() && _connectionStateService.GetExportCompletedStatus()))
            ? Visibility.Collapsed : Visibility.Visible;

        _logService.Debug($"Visibilidade do botão Cancelar atualizada para: {IsCancelButtonVisible}");
    }

    /// <summary>
    /// Método centralizado para atualizar a visibilidade de todos os botões.
    /// </summary>
    private void UpdateAllButtonVisibilities()
    {
        UpdateNextButtonVisibility();
        UpdatePreviousButtonVisible();
        UpdateFinishButtonVisible();
        UpdateNovaButtonVisible();  
        UpdateCloseButtonVisible();
        UpdateCancelButtonVisible();
    }

    /// <summary>
    /// Libera recursos e desassina eventos para evitar memory leaks.
    /// </summary>
    public void Dispose()
    {
        _logService.Info("MainWindowViewModel Dispose chamado. Desassinando eventos.");

        _connectionStateService.ConnectionStatusChanged -= OnConnectionStatusChanged;
        _connectionStateService.ExportCompletedStatusChanged -= OnExportCompletedStatusChanged;

        if (_connectionTestViewModelPropertyChangedHandler != null)
            _connectionTestViewModel.PropertyChanged -= _connectionTestViewModelPropertyChangedHandler;

        if (_entitySelectionViewModelPropertyChangedHandler != null)
            _entitySelectionViewModel.PropertyChanged -= _entitySelectionViewModelPropertyChangedHandler;

        if (_destinationDirectoryViewModelPropertyChangedHandler != null)
            _destinationDirectoryViewModel.PropertyChanged -= _destinationDirectoryViewModelPropertyChangedHandler;

        if (_exportProcessViewModelPropertyChangedHandler != null)
            _exportProcessViewModel.PropertyChanged -= _exportProcessViewModelPropertyChangedHandler;

        GC.SuppressFinalize(this);
    }
}
