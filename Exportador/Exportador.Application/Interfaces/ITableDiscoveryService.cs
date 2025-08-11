using CSharpFunctionalExtensions;
using Exportador.Core.ValueObjects;  

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define a interface para serviços de descoberta de nomes de tabelas em um banco de dados.
/// </summary>
public interface ITableDiscoveryService
{
    /// <summary>
    /// Busca os nomes das tabelas em um banco de dados com base nos parâmetros de conexão e um padrão de pesquisa.
    /// </summary>
    /// <param name="connectionParameters">Objeto de valor contendo os parâmetros de conexão com o banco de dados.</param>
    /// <param name="searchPattern">Padrão SQL LIKE para filtrar os nomes das tabelas (ex: "%Cliente%").</param>
    /// <returns>Um Result contendo uma lista de nomes de tabelas se a operação for bem-sucedida,
    /// ou uma mensagem de erro em caso de falha.</returns>
    Task<Result<List<string>>> GetTableNamesAsync(ConnectionParameters connectionParameters, string searchPattern);
}