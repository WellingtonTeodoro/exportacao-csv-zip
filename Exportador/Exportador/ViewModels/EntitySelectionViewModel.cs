using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exportador.Application.Interfaces;
using System.Collections.ObjectModel;

namespace Exportador.UI.ViewModels;

/// <summary>
/// ViewModel para a tela de seleção de entidades (tabelas) para exportação.
/// Gerencia o estado de uma coleção dinâmica de entidades selecionáveis e interage com os serviços e casos de uso
/// para persistir o estado de seleção.
/// </summary>
public partial class EntitySelectionViewModel : ObservableObject
{
    private readonly ILogService _logService;
    private readonly IManageSelectedTablesUseCase _manageSelectedTablesUseCase;
    private readonly IConnectionStateService _connectionStateService;

    /// <summary>
    /// Coleção de todas as entidades que estão disponíveis para seleção na UI,
    /// incluindo seus nomes, estados de seleção e caminhos de imagem.
    /// </summary>
    public ObservableCollection<SelectableEntity> AvailableEntities { get; }

    /// <summary>
    /// Indica se alguma operação assíncrona interna à tela de seleção está em andamento.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing;

    /// <summary>
    /// Obtém uma mensagem formatada que exibe a contagem de entidades selecionadas atualmente.
    /// A contagem é dinâmica, baseada nos itens da coleção <see cref="AvailableEntities"/>.
    /// </summary>
    public string SelectedEntitiesCountMessage
    {
        get
        {
            int selectedCount = AvailableEntities.Count(e => e.IsSelected);
            return $"Entidades selecionadas: {selectedCount} de {AvailableEntities.Count}";
        }
    }

    /// <summary>
    /// Indica se o botão "Próximo" deve ser habilitado.
    /// É baseada na conexão ativa e na existência de pelo menos uma entidade selecionada.
    /// </summary>
    public bool CanProceed
    {
        get
        {
            return _connectionStateService.IsConnected && AvailableEntities.Any(e => e.IsSelected);
        }
    }

    /// <summary>
    /// Construtor do ViewModel. Realiza a injeção de dependências e inicializa a coleção de entidades.
    /// </summary>
    /// <param name="logService">Serviço para registro de logs.</param>
    /// <param name="manageSelectedTablesUseCase">Caso de uso para gerenciar tabelas selecionadas.</param>
    /// <param name="connectionStateService">Serviço para monitorar o estado da conexão.</param>
    /// <exception cref="ArgumentNullException">Lançada se qualquer uma das dependências for nula.</exception>
    public EntitySelectionViewModel(
        ILogService logService,
        IManageSelectedTablesUseCase manageSelectedTablesUseCase,
        IConnectionStateService connectionStateService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _manageSelectedTablesUseCase = manageSelectedTablesUseCase ?? throw new ArgumentNullException(nameof(manageSelectedTablesUseCase));
        _connectionStateService = connectionStateService ?? throw new ArgumentNullException(nameof(connectionStateService));

        _logService.Info("EntitySelectionViewModel inicializado (versão SOLID completa com imagens).");

        // Inicializa a coleção de entidades disponíveis COM os caminhos das imagens.
        AvailableEntities = new ObservableCollection<SelectableEntity>
        {
            new SelectableEntity("Clientes", false, "/Images/cliente.png"),
            new SelectableEntity("Produtos", false, "/Images/product.png"),
            new SelectableEntity("ContasReceber", false, "/Images/add.png"),
            new SelectableEntity("ContasPagar", false, "/Images/sub.png"),
            new SelectableEntity("NFe", false, "/Images/history.png"),  
            new SelectableEntity("NFCe", false, "/Images/history.png"),  
            new SelectableEntity("Notas", false, "/Images/history.png")   
        };

        _connectionStateService.ConnectionStatusChanged += OnConnectionStatusChanged;

        foreach (var entity in AvailableEntities)
        {
            entity.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectableEntity.IsSelected))
                { 
                    if (sender is SelectableEntity changedEntity)
                    {
                        UpdateEntitySelection(changedEntity.Name, changedEntity.IsSelected);
                         
                        OnPropertyChanged(nameof(SelectedEntitiesCountMessage));
                        OnPropertyChanged(nameof(CanProceed));
                    }
                    else
                    { 
                        _logService.Warning($"PropertyChanged disparado por um objeto inesperado ou nulo para IsSelected. Tipo: {sender?.GetType().Name ?? "null"}");
                    }
                }
            };
        }

        LoadInitialSelectionState();
    }

    /// <summary>
    /// Método de callback invocado quando o status da conexão muda.
    /// Notifica a mudança na propriedade <see cref="CanProceed"/>.
    /// </summary>
    /// <param name="isConnected">Indica se a conexão foi estabelecida com sucesso.</param>
    private void OnConnectionStatusChanged(bool isConnected)
    {
        OnPropertyChanged(nameof(CanProceed));
    }

    /// <summary>
    /// Carrega o estado inicial de seleção dos checkboxes na UI com base nas entidades
    /// que já estão selecionadas e armazenadas no repositório (via caso de uso).
    /// </summary>
    private void LoadInitialSelectionState()
    {
        var selectedNamesFromRepo = new HashSet<string>(_manageSelectedTablesUseCase.GetSelectedTables());

        foreach (var entity in AvailableEntities)
        {
            entity.IsSelected = selectedNamesFromRepo.Contains(entity.Name);
        }

        OnPropertyChanged(nameof(SelectedEntitiesCountMessage));
        OnPropertyChanged(nameof(CanProceed));

        _logService.Debug("Estado inicial dos checkboxes carregado do repositório.");
    }

    /// <summary>
    /// Método auxiliar para adicionar ou remover um nome de entidade do repositório.
    /// Este método é chamado quando o estado <see cref="SelectableEntity.IsSelected"/> de uma entidade muda.
    /// </summary>
    /// <param name="entityName">O nome da entidade (tabela) a ser gerenciada.</param>
    /// <param name="isSelected">True para adicionar (selecionar), False para remover (desselecionar).</param>
    private void UpdateEntitySelection(string entityName, bool isSelected)
    {
        _logService.Debug($"Atualizando seleção para '{entityName}'. Novo estado: {isSelected}.");

        if (isSelected)
        {
            var result = _manageSelectedTablesUseCase.AddTable(entityName);
            if (result.IsFailure)
            {
                _logService.Error($"Falha ao adicionar a entidade '{entityName}': {result.Error}");
            }
        }
        else
        {
            var result = _manageSelectedTablesUseCase.RemoveTable(entityName);
            if (result.IsFailure)
            {
                _logService.Error($"Falha ao remover a entidade '{entityName}': {result.Error}");
            }
        }
    }

    /// <summary>
    /// Comando para selecionar todas as entidades.
    /// Itera sobre a coleção <see cref="AvailableEntities"/> e define <see cref="SelectableEntity.IsSelected"/> para true.
    /// </summary>
    [RelayCommand]
    private void SelectAll()
    {
        foreach (var entity in AvailableEntities)
        {
            entity.IsSelected = true;
        }
        _logService.Info("Todas as entidades foram selecionadas.");
    }

    /// <summary>
    /// Comando para limpar a seleção de todas as entidades.
    /// Itera sobre a coleção <see cref="AvailableEntities"/> e define <see cref="SelectableEntity.IsSelected"/> para false.
    /// </summary>
    [RelayCommand]
    private void ClearSelection()
    {
        foreach (var entity in AvailableEntities)
        {
            entity.IsSelected = false;
        }
        _logService.Info("A seleção de entidades foi limpa.");
    }

    /// <summary>
    /// Reseta o estado da EntitySelectionViewModel para o seu ponto inicial,
    /// limpando todas as seleções e atualizando a UI.
    /// </summary>
    public void Reset()
    {
        _logService.Info("EntitySelectionViewModel.Reset() chamado. Limpando seleções.");
         
        _manageSelectedTablesUseCase.ClearSelectedEntities();
         
        foreach (var entity in AvailableEntities)
        {
            entity.IsSelected = false;
        }
         
        IsProcessing = false;
         
        OnPropertyChanged(nameof(SelectedEntitiesCountMessage));
        OnPropertyChanged(nameof(CanProceed));

        _logService.Info("EntitySelectionViewModel resetado com sucesso.");
    }
}