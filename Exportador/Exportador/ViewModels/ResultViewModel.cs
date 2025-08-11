using CommunityToolkit.Mvvm.ComponentModel;
using Exportador.Application.Interfaces;
using Exportador.Core.Models;
using System.Collections.ObjectModel;

namespace Exportador.UI.ViewModels;

/// <summary>
/// ViewModel responsável por gerenciar e fornecer os dados do resultado da exportação para a interface do usuário.
/// Segue o padrão MVVM com notificações para atualizar a View automaticamente.
/// </summary>
public class ResultViewModel : ObservableObject
{
    private readonly IExportResultRepository _exportResultRepository;
    /// <summary>
    /// Simplesmente pra usar log na view.
    /// </summary>
    public ILogService _logService { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ResultViewModel"/>.
    /// </summary>
    /// <param name="exportResultRepository">Repositório para persistência e carregamento dos dados do resultado da exportação.</param>
    /// <param name="logService"></param>
    public ResultViewModel(IExportResultRepository exportResultRepository, ILogService logService)
    {
        _exportResultRepository = exportResultRepository;
        _logService = logService;
    }

    /// <summary>
    /// Coleção observável contendo as informações das tabelas exportadas.
    /// Atualiza a View automaticamente ao adicionar ou remover itens.
    /// </summary>
    public ObservableCollection<ExportedTableInfo> ExportedTables { get; } = new();

    private int _totalRecords;

    /// <summary>
    /// Quantidade total de registros exportados.
    /// </summary>
    public int TotalRecords
    {
        get => _totalRecords;
        set => SetProperty(ref _totalRecords, value);
    }

    private string _zipFileName = string.Empty;

    /// <summary>
    /// Nome do arquivo ZIP gerado com os dados exportados.
    /// </summary>
    public string ZipFileName
    {
        get => _zipFileName;
        set => SetProperty(ref _zipFileName, value);
    }

    private long _zipFileSizeBytes;

    /// <summary>
    /// Tamanho do arquivo ZIP em bytes.
    /// </summary>
    public long ZipFileSizeBytes
    {
        get => _zipFileSizeBytes;
        set
        {
            if (SetProperty(ref _zipFileSizeBytes, value))
            { 
                OnPropertyChanged(nameof(ZipFileSizeDisplay));
            }
        }
    }

    private string _totalExportTime = string.Empty;

    /// <summary>
    /// Tempo total gasto na exportação, formatado como string (exemplo: "2m 34s").
    /// </summary>
    public string TotalExportTime
    {
        get => _totalExportTime;
        set => SetProperty(ref _totalExportTime, value);
    }

    /// <summary>
    /// Representação formatada do tamanho do arquivo ZIP em megabytes (MB) com uma casa decimal.
    /// Exemplo: "18.5 MB".
    /// </summary>
    public string ZipFileSizeDisplay => $"{ZipFileSizeBytes / 1024.0 / 1024.0:N1} MB";

    /// <summary>
    /// Carrega os dados do último resultado de exportação salvo no repositório e atualiza as propriedades para refletir na View.
    /// </summary>
    /// <returns>Task assíncrona representando a operação de carregamento.</returns>
    public async Task LoadAsync()
    {
        var result = await _exportResultRepository.LoadLatestAsync();
        if (result == null)
        { 
            Reset();
            _logService.Info("LoadAsync: Nenhum resultado de exportação anterior encontrado, resetando ViewModel.");
            return;
        }

        ExportedTables.Clear();
        foreach (var t in result.ExportedTables)
            ExportedTables.Add(t);

        TotalRecords = result.TotalRecords;
        ZipFileName = result.ZipFileName;
        ZipFileSizeBytes = result.ZipFileSizeBytes;
         
        TotalExportTime = result.TotalExportTime.ToString(@"m\m\ s\s");
        _logService.Info($"LoadAsync: Dados da exportação anterior carregados. Arquivo: {ZipFileName}");
    }

    /// <summary>
    /// Reseta o estado do ResultViewModel, limpando todos os dados da exportação anterior
    /// e retornando as propriedades para seus valores iniciais.
    /// </summary>
    public void Reset()
    {
        _logService.Info("ResultViewModel.Reset() chamado. Limpando dados da exportação anterior.");
         
        ExportedTables.Clear();
         
        TotalRecords = 0;
         
        ZipFileName = string.Empty;
         
        ZipFileSizeBytes = 0;
         
        TotalExportTime = string.Empty;

        _logService.Info("ResultViewModel resetado com sucesso.");
    }
}
