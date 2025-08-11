using CSharpFunctionalExtensions;  
using Exportador.Core.ValueObjects;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define um serviço para recuperação de dados de fontes de dados (ex: banco de dados SQL).
/// </summary>
public interface IDataRetrievalService
{
    /// <summary>
    /// Recupera dados de uma tabela específica do banco de dados, selecionando colunas específicas.
    /// </summary>
    /// <param name="connectionParameters">Os parâmetros de conexão com o banco de dados.</param>
    /// <param name="tableName">O nome real da tabela no banco de dados (incluindo schema, se aplicável).</param>
    /// <param name="selectColumns">Uma string contendo as colunas a serem selecionadas (ex: "Id, Nome, Descricao").</param>
    /// <returns>Um Result contendo uma lista de dicionários (dados) em caso de sucesso, ou uma mensagem de erro em caso de falha.</returns>
    // Note que a exceção ApplicationException na documentação foi removida,
    // pois o retorno Result elimina a necessidade de lançá-la diretamente.
    Task<Result<List<Dictionary<string, object>>>> GetDataFromTableAsync(ConnectionParameters connectionParameters, string tableName, string selectColumns);
}