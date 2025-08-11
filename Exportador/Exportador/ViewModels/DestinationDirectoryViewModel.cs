using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exportador.Application.Interfaces;
using Exportador.UI.Interfaces;
using System.IO;

namespace Exportador.UI.ViewModels;

/// <summary>
/// ViewModel responsável pela seleção do diretório de destino da exportação.
/// </summary>
public partial class DestinationDirectoryViewModel : ObservableRecipient
{
    private readonly IFolderBrowserDialogService _folderBrowserDialogService;
    private readonly IExportSettingsRepository _exportSettingsRepository;
    private readonly ILogService _logService;

    /// <summary>
    /// Obtém ou define o caminho do diretório de destino selecionado pelo usuário.
    /// Notifica a UI e recalcula CanProceed e FullExportFilePath.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BrowseDestinationDirectoryCommand))]
    [NotifyPropertyChangedFor(nameof(CanProceed))]
    [NotifyPropertyChangedFor(nameof(FullExportFilePath))]
    private string _selectedDirectory = string.Empty;

    /// <summary>
    /// Obtém ou define o nome do arquivo de saída.
    /// Notifica a UI e recalcula FullExportFilePath, E SALVA A ALTERAÇÃO.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullExportFilePath))]
    [NotifyPropertyChangedRecipients]  
    private string _outputFileName = string.Empty;
     
    partial void OnOutputFileNameChanged(string value)
    { 
        _exportSettingsRepository.SetOutputFileName(value);
        _logService.Debug($"Nome do arquivo de saída atualizado para: '{value}' e salvo.");
    }


    /// <summary>
    /// Indica se é possível prosseguir para a próxima etapa (se um diretório válido foi selecionado).
    /// </summary>
    public bool CanProceed => !string.IsNullOrEmpty(SelectedDirectory) && Directory.Exists(SelectedDirectory);

    /// <summary>
    /// Obtém o caminho completo do arquivo de exportação (Diretório + Nome do arquivo).
    /// </summary>
    public string FullExportFilePath
    {
        get
        {
            if (!string.IsNullOrEmpty(SelectedDirectory) && !string.IsNullOrEmpty(OutputFileName))
            { 
                return Path.Combine(SelectedDirectory, OutputFileName + ".zip");
            }
            return "Caminho de exportação não definido.";
        }
    }

    /// <summary>
    /// Construtor para DestinationDirectoryViewModel.
    /// </summary>
    public DestinationDirectoryViewModel(
        IFolderBrowserDialogService folderBrowserDialogService,
        IExportSettingsRepository exportSettingsRepository,
        ILogService logService)
    {
        _folderBrowserDialogService = folderBrowserDialogService ?? throw new ArgumentNullException(nameof(folderBrowserDialogService));
        _exportSettingsRepository = exportSettingsRepository ?? throw new ArgumentNullException(nameof(exportSettingsRepository));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));

        LoadExportSettings();

        _logService.Info("DestinationDirectoryViewModel inicializado.");
    }

    /// <summary>
    /// Carrega as últimas configurações de exportação (diretório e nome do arquivo).
    /// </summary>
    private void LoadExportSettings()
    {
        try
        {
            string lastPath = _exportSettingsRepository.GetDestinationDirectoryPath();
            if (!string.IsNullOrEmpty(lastPath) && Directory.Exists(lastPath))
            {
                SelectedDirectory = lastPath;
                _logService.Debug($"Último diretório de exportação carregado: {SelectedDirectory}");
            }
            else
            {
                _logService.Debug("Nenhum último diretório de exportação válido encontrado nas configurações.");
                SelectedDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ExportadorFusion");
            }

            string lastFileName = _exportSettingsRepository.GetOutputFileName();
            if (!string.IsNullOrEmpty(lastFileName))
            {
                OutputFileName = lastFileName;  
                _logService.Debug($"Último nome de arquivo de exportação carregado: {OutputFileName}");
            }
            else
            {
                _logService.Debug("Nenhum último nome de arquivo de exportação válido encontrado nas configurações.");
                OutputFileName = "novo_arquivo_export"; 
            }
        }
        catch (Exception ex)
        {
            _logService.Error($"Erro ao carregar configurações de exportação: {ex.Message}");
        }
    }

    /// <summary>
    /// Comando para abrir o diálogo de seleção de diretório E definir como destino.
    /// </summary>
    [RelayCommand]
    private void BrowseDestinationDirectory()
    {
        var selectedPath = _folderBrowserDialogService.SelectFolder(SelectedDirectory);

        if (!string.IsNullOrEmpty(selectedPath))
        {
            SelectedDirectory = selectedPath; 
            _exportSettingsRepository.SetDestinationDirectoryPath(selectedPath);
            _logService.Info($"Diretório de destino selecionado: {SelectedDirectory}");
        }
        else
        {
            _logService.Info("Seleção de diretório de destino cancelada.");
        }
    }
}