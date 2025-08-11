using Exportador.Application.Interfaces;
using CSharpFunctionalExtensions;

namespace Exportador.Application.UseCases;

/// <summary>
/// Caso de uso responsável por gerenciar a adição e remoção de nomes de tabelas selecionadas
/// no repositório de dados.
/// </summary>
public class ManageSelectedTables : IManageSelectedTablesUseCase
{
    private readonly IDataRepositoryService _dataRepositoryService;
    private readonly ILogService _logService;

    /// <summary>
    /// Construtor do caso de uso.
    /// </summary>
    /// <param name="dataRepositoryService">O serviço de repositório de dados para gerenciar as seleções.</param>
    /// <param name="logService">O serviço de log.</param>
    /// <exception cref="ArgumentNullException">Lançada se os serviços injetados forem nulos.</exception>
    public ManageSelectedTables(IDataRepositoryService dataRepositoryService, ILogService logService)
    {
        _dataRepositoryService = dataRepositoryService ?? throw new ArgumentNullException(nameof(dataRepositoryService));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    /// <summary>
    /// Adiciona um nome de tabela ao repositório de tabelas selecionadas.
    /// </summary>
    /// <param name="tableName">O nome da tabela a ser adicionada.</param>
    /// <returns>Um Result indicando sucesso ou falha.</returns>
    public Result AddTable(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            _logService.Warning("Tentativa de adicionar nome de tabela nulo ou vazio."); 
            return Result.Failure("O nome da tabela não pode ser vazio.");
        }

        try
        {
            _dataRepositoryService.AddSelectedEntity(tableName);
            _logService.Debug($"Tabela '{tableName}' adicionada via caso de uso ManageSelectedTables.");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logService.Error(ex, $"Erro ao adicionar tabela '{tableName}'.");
            return Result.Failure($"Erro ao adicionar tabela: {ex.Message}");
        }
    }

    /// <summary>
    /// Remove um nome de tabela do repositório de tabelas selecionadas.
    /// </summary>
    /// <param name="tableName">O nome da tabela a ser removida.</param>
    /// <returns>Um Result indicando sucesso ou falha.</returns>
    public Result RemoveTable(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            _logService.Warning("Tentativa de remover nome de tabela nulo ou vazio."); 
            return Result.Failure("O nome da tabela não pode ser vazio.");
        }

        try
        {
            _dataRepositoryService.RemoveSelectedEntity(tableName);
            _logService.Debug($"Tabela '{tableName}' removida via caso de uso ManageSelectedTables.");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logService.Error(ex, $"Erro ao remover tabela '{tableName}'.");
            return Result.Failure($"Erro ao remover tabela: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém a lista de tabelas atualmente selecionadas.
    /// </summary>
    /// <returns>Uma lista somente leitura dos nomes das tabelas selecionadas.</returns>
    public IReadOnlyList<string> GetSelectedTables()
    { 
        return _dataRepositoryService.GetSelectedEntityNames();
    }

    /// <summary>
    /// Limpa todas as entidades selecionadas no repositório de dados.
    /// </summary>
    public void ClearSelectedEntities()
    { 
        _dataRepositoryService.ClearSelectedEntities();
        _logService.Debug("IManageSelectedTablesUseCase: Entidades selecionadas limpas no repositório.");
    }
}