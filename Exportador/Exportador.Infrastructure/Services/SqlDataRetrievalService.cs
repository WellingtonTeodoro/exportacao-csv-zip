using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;
using Exportador.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

#nullable disable

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Serviço responsável por recuperar dados de uma tabela SQL Server específica, com base em parâmetros de conexão.
/// Implementa <see cref="IDataRetrievalService"/>.
/// </summary>
public class SqlDataRetrievalService : IDataRetrievalService
{
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="SqlDataRetrievalService"/>.
    /// </summary>
    /// <param name="logService">Serviço de logging utilizado para registrar eventos, erros e diagnósticos.</param>
    /// <exception cref="ArgumentNullException">Lançado se <paramref name="logService"/> for nulo.</exception>
    public SqlDataRetrievalService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("SqlDataRetrievalService inicializado.");
    }

    /// <summary>
    /// Recupera os dados de uma tabela do banco de dados com base nas colunas e parâmetros de conexão fornecidos.
    /// </summary>
    /// <param name="connectionParameters">Parâmetros de conexão, como servidor, banco, usuário e senha.</param>
    /// <param name="tableName">Nome da tabela a ser consultada.</param>
    /// <param name="selectColumns">Lista de colunas a serem selecionadas, separadas por vírgula.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo uma lista de dicionários onde cada item representa uma linha,
    /// e cada chave/valor representa o nome da coluna e seu respectivo valor (pode ser null).
    /// </returns>
    public async Task<Result<List<Dictionary<string, object>>>> GetDataFromTableAsync(
        ConnectionParameters connectionParameters,
        string tableName,
        string selectColumns)
    { 
        if (connectionParameters == null)
        {
            _logService.Error("Parâmetros de conexão nulos fornecidos para GetDataFromTableAsync.");
            return Result.Failure<List<Dictionary<string, object>>>("Os parâmetros de conexão não podem ser nulos.");
        }

        if (string.IsNullOrWhiteSpace(tableName))
        {
            _logService.Error("Nome da tabela nulo ou vazio fornecido para GetDataFromTableAsync.");
            return Result.Failure<List<Dictionary<string, object>>>("O nome da tabela não pode ser nulo ou vazio.");
        }
         
        if (string.IsNullOrWhiteSpace(selectColumns))
        {
            selectColumns = "*";
        }

        var data = new List<Dictionary<string, object>>();
        var connectionString = SqlConnectionStringBuilderHelper.Build(connectionParameters, true);

        string formattedTableName; 
        string[] parts = tableName.Split(new[] { '.' }, 2); 

        if (parts.Length == 2)
        { 
            formattedTableName = $"[{parts[0]}].[{parts[1]}]";
        }
        else
        { 
            formattedTableName = $"[{tableName}]";
            _logService.Warning($"Nome de tabela '{tableName}' fornecido sem qualificação de esquema. Assumindo esquema padrão ou contexto de banco de dados.");
        }

        string query = $"SELECT {selectColumns} FROM {formattedTableName}";  

        try
        {
            _logService.Debug($"Connection String sendo usada: {connectionString}");
            _logService.Debug($"Executando query para tabela '{tableName}': {query}");
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            _logService.Debug($"Conexão com o banco de dados aberta para {connectionParameters.Database} em {connectionParameters.Server}.");

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
             
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    row.Add(reader.GetName(i), value);
                }
                data.Add(row);
            }

            _logService.Info($"Dados da tabela '{tableName}' recuperados com sucesso. Total de {data.Count} registros.");
            return Result.Success(data);
        }
        catch (SqlException ex)
        {
            _logService.Error(ex, $"Erro SQL ao recuperar dados da tabela '{tableName}'. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}. " +
                                  $"Código do Erro SQL: {ex.Number}. Mensagem: {ex.Message}. " +
                                  $"Query executada: {query}");
            return Result.Failure<List<Dictionary<string, object>>>($"Falha ao recuperar dados da tabela '{tableName}' (Código SQL: {ex.Number}): {ex.Message}");
        }
        catch (Exception ex)
        {
            _logService.Error(ex, $"Erro inesperado ao recuperar dados da tabela '{tableName}'. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}. " +
                                  $"Mensagem: {ex.Message}. " +
                                  $"Query executada: {query}");
            return Result.Failure<List<Dictionary<string, object>>>($"Falha inesperada ao recuperar dados da tabela '{tableName}': {ex.Message}");
        }
    }
}