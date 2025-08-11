namespace Exportador.Application.Interfaces;

/// <summary>
/// Interface responsável por definir operações de acesso e armazenamento temporário
/// dos nomes das entidades (tabelas) selecionadas para exportação.
/// Não lida com a recuperação dos dados reais das tabelas, apenas com seus nomes.
/// </summary>
public interface IDataRepositoryService
{
    /// <summary>
    /// Adiciona o nome de uma entidade (tabela) à lista de seleções.
    /// </summary>
    /// <param name="entityName">Nome lógico da entidade a ser adicionada.</param>
    void AddSelectedEntity(string entityName);

    /// <summary>
    /// Remove o nome de uma entidade (tabela) da lista de seleções.
    /// </summary>
    /// <param name="entityName">Nome lógico da entidade a ser removida.</param>
    void RemoveSelectedEntity(string entityName);

    /// <summary>
    /// Retorna todos os nomes de entidades (tabelas) atualmente selecionados.
    /// </summary>
    /// <returns>Uma lista somente leitura contendo os nomes das entidades selecionadas.</returns>
    IReadOnlyList<string> GetSelectedEntityNames();

    /// <summary>
    /// Verifica se uma entidade específica está selecionada.
    /// </summary>
    /// <param name="entityName">Nome lógico da entidade.</param>
    /// <returns>True se a entidade estiver selecionada, False caso contrário.</returns>
    bool IsEntitySelected(string entityName);

    /// <summary>
    /// Limpa todas as entidades selecionadas do repositório.
    /// </summary>
    void ClearSelectedEntities();
}