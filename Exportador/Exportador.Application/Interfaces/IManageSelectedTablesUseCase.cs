using CSharpFunctionalExtensions;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define o contrato para casos de uso que gerenciam a seleção de tabelas para exportação.
/// Garante que a camada de UI (ViewModels) dependa de uma abstração,
/// facilitando testes unitários, a inversão de controle e a substituição de implementações.
/// </summary>
public interface IManageSelectedTablesUseCase
{
    /// <summary>
    /// Adiciona um nome de tabela à lista de tabelas selecionadas para exportação.
    /// </summary>
    /// <param name="tableName">O nome da tabela a ser adicionada.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha da operação. Se falhar, contém uma mensagem de erro.</returns>
    Result AddTable(string tableName);

    /// <summary>
    /// Remove um nome de tabela da lista de tabelas selecionadas para exportação.
    /// </summary>
    /// <param name="tableName">O nome da tabela a ser removida.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha da operação. Se falhar, contém uma mensagem de erro.</returns>
    Result RemoveTable(string tableName);

    /// <summary>
    /// Obtém a lista atual de tabelas selecionadas.
    /// </summary>
    /// <returns>Uma lista somente leitura (<see cref="IReadOnlyList{T}"/>) contendo os nomes das tabelas selecionadas.</returns>
    IReadOnlyList<string> GetSelectedTables();

    /// <summary>
    /// Limpa todas as entidades (tabelas) que foram selecionadas e armazenadas.
    /// </summary>
    void ClearSelectedEntities();
}