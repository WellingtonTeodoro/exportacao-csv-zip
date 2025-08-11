using Exportador.Application.Interfaces;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Serviço responsável por manter os nomes das entidades (tabelas) selecionadas para exportação.
/// Não armazena os dados reais das tabelas, apenas seus nomes para orquestração futura.
/// </summary>
public class DataRepositoryService : IDataRepositoryService
{
    private readonly ILogService _logService; 
     
    private readonly HashSet<string> _selectedEntityNames;

    /// <summary>
    /// Inicializa uma nova instância do serviço de repositório de dados.
    /// </summary>
    public DataRepositoryService(ILogService logService)  
    {
        _logService = logService;
        _selectedEntityNames = new HashSet<string>();

        _logService.Info("DataRepositoryService inicializado para gerenciar nomes de entidades selecionadas.");
    }

    /// <summary>
    /// Adiciona um nome de entidade (tabela) à lista de seleções.
    /// </summary>
    /// <param name="entityName">Nome lógico da entidade (ex: "Clientes")</param>
    public void AddSelectedEntity(string entityName)
    {
        if (string.IsNullOrWhiteSpace(entityName))
        {
            _logService.Warning("Tentativa de adicionar nome de entidade nulo ou vazio ao repositório.");
            return;
        }

        if (_selectedEntityNames.Add(entityName))
        {
            _logService.Debug($"Entidade '{entityName}' adicionada às seleções.");
        }
        else
        {
            _logService.Debug($"Entidade '{entityName}' já estava selecionada.");
        }
    }

    /// <summary>
    /// Remove um nome de entidade (tabela) da lista de seleções.
    /// </summary>
    /// <param name="entityName">Nome lógico da entidade (ex: "Clientes")</param>
    public void RemoveSelectedEntity(string entityName)
    {
        if (string.IsNullOrWhiteSpace(entityName))
        {
            _logService.Warning("Tentativa de remover nome de entidade nulo ou vazio do repositório.");
            return;
        }

        if (_selectedEntityNames.Remove(entityName))
        {
            _logService.Debug($"Entidade '{entityName}' removida das seleções.");
        }
        else
        {
            _logService.Debug($"Entidade '{entityName}' não estava selecionada.");
        }
    }

    /// <summary>
    /// Retorna todos os nomes de entidades (tabelas) atualmente selecionados.
    /// </summary>
    /// <returns>Uma lista contendo os nomes das entidades selecionadas.</returns>
    public IReadOnlyList<string> GetSelectedEntityNames()
    {
        return _selectedEntityNames.ToList().AsReadOnly();
    }

    /// <summary>
    /// Verifica se uma entidade específica está selecionada.
    /// </summary>
    /// <param name="entityName">Nome lógico da entidade.</param>
    /// <returns>True se a entidade estiver selecionada, False caso contrário.</returns>
    public bool IsEntitySelected(string entityName)
    {
        return _selectedEntityNames.Contains(entityName);
    }

    /// <summary>
    /// Limpa todas as entidades selecionadas do repositório.
    /// </summary>
    public void ClearSelectedEntities()
    {
        int count = _selectedEntityNames.Count;
        _selectedEntityNames.Clear();
        _logService.Debug($"Total de {count} entidades selecionadas foram limpas do repositório.");
    } 
}