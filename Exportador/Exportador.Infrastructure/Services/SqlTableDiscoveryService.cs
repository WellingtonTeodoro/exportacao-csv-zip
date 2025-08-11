using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;
using Microsoft.Data.SqlClient;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Implementação de <see cref="ITableDiscoveryService"/>  
/// </summary>
public class SqlTableDiscoveryService : ITableDiscoveryService
{
    private readonly ILogService _logService;

    /// <summary>
    /// Construtor de <see cref="SqlTableDiscoveryService"/>.
    /// </summary>
    /// <param name="logService">Serviço de log para registrar informações e erros.</param>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="logService"/> for nulo.</exception>
    public SqlTableDiscoveryService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("SqlTableDiscoveryService inicializado.");
    }

    /// <summary>
    /// Busca os nomes das tabelas em um banco de dados SQL Server.
    /// </summary>
    /// <param name="connectionParameters">Objeto de valor com os parâmetros de conexão.</param>
    /// <param name="searchPattern">Padrão SQL LIKE para filtrar os nomes das tabelas.</param>
    /// <returns>Um Result contendo uma lista de nomes de tabelas se a operação for bem-sucedida,
    /// ou uma mensagem de erro em caso de falha.</returns>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="connectionParameters"/> for nulo.</exception>
    public async Task<Result<List<string>>> GetTableNamesAsync(ConnectionParameters connectionParameters, string searchPattern)
    {
        if (connectionParameters == null)
            throw new ArgumentNullException(nameof(connectionParameters), "Os parâmetros de conexão não podem ser nulos.");
        if (searchPattern == null)
            searchPattern = string.Empty;

        var foundTables = new List<string>();
        var connectionString = Helpers.SqlConnectionStringBuilderHelper.Build(connectionParameters, true);

        try
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();  

            var query = @"
                SELECT TABLE_SCHEMA + '.' + TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME LIKE @pattern";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@pattern", searchPattern);

            using var reader = await command.ExecuteReaderAsync();  
            while (await reader.ReadAsync())
            {
                foundTables.Add(reader.GetString(0));
            }

            _logService.Info($"Descoberta de tabelas SQL concluída para padrão '{searchPattern}'. {foundTables.Count} tabelas encontradas.");
            return Result.Success(foundTables);
        }
        catch (SqlException ex)
        { 
            _logService.Error(ex, $"SqlException ao buscar tabelas com padrão '{searchPattern}'. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}. " +
                                  $"Código do Erro SQL: {ex.Number}.");
            return Result.Failure<List<string>>($"Erro de banco de dados (Código SQL: {ex.Number}): {ex.Message}");
        }
        catch (Exception ex)
        {
            _logService.Error(ex, $"Erro inesperado ao buscar tabelas com padrão '{searchPattern}'. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}.");
            return Result.Failure<List<string>>($"Erro inesperado ao buscar tabelas: {ex.Message}");
        }
    }
}