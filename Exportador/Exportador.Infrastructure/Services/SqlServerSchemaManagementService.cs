using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;
using Microsoft.Data.SqlClient;
using Exportador.Infrastructure.Helpers;  

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Serviço responsável por criar ou alterar views em bancos de dados SQL Server.
/// </summary>
public class SqlServerSchemaManagementService : ISchemaManagementService
{
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância do serviço de gerenciamento de schema para SQL Server.
    /// </summary>
    /// <param name="logService">Serviço de log para registrar ações e erros.</param>
    /// <exception cref="ArgumentNullException">Lançado se <paramref name="logService"/> for nulo.</exception>
    public SqlServerSchemaManagementService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("SqlServerSchemaManagementService inicializado.");
    }

    /// <summary>
    /// Cria ou recria uma view no banco de dados.
    /// Caso a view já exista, ela será descartada antes de ser criada novamente com a nova definição SQL.
    /// </summary>
    /// <param name="connectionParameters">Parâmetros de conexão com o banco de dados.</param>
    /// <param name="viewName">Nome da view a ser criada ou alterada.</param>
    /// <param name="viewDefinitionSql">Definição SQL da view (sem o comando CREATE VIEW).</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha. Retorna <see cref="Result.Failure"/> com uma mensagem de erro em caso de falha.</returns>
    /// <exception cref="ArgumentNullException">Lançado se <paramref name="connectionParameters"/> for nulo (melhor verificar aqui).</exception>
    public async Task<Result> CreateOrAlterViewAsync(ConnectionParameters connectionParameters, string viewName, string viewDefinitionSql)
    { 
        if (connectionParameters == null)
            throw new ArgumentNullException(nameof(connectionParameters), "Parâmetros de conexão não podem ser nulos.");
        if (string.IsNullOrWhiteSpace(viewName))
            throw new ArgumentException("O nome da view não pode ser nulo ou vazio.", nameof(viewName));
        if (string.IsNullOrWhiteSpace(viewDefinitionSql))
            throw new ArgumentException("A definição SQL da view não pode ser nula ou vazia.", nameof(viewDefinitionSql));
         
        string connectionString = SqlConnectionStringBuilderHelper.Build(connectionParameters, trustServerCertificate: true);

        _logService.Info($"Iniciando operação para view: {viewName} no servidor {connectionParameters.Server}, banco {connectionParameters.Database}.");

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                _logService.Debug($"Conexão aberta para criar/alterar view: {viewName}.");
                 
                string dropViewIfExistsSql = $@"
                        IF OBJECT_ID('{viewName}', 'V') IS NOT NULL
                        BEGIN
                            DROP VIEW {viewName};
                        END;";

                _logService.Debug($"Executando DROP VIEW IF EXISTS para: {viewName}");
                using (var dropCommand = new SqlCommand(dropViewIfExistsSql, connection))
                {
                    await dropCommand.ExecuteNonQueryAsync();
                }
                _logService.Info($"Verificação e descarte (se existia) da view '{viewName}' concluídos.");
                 
                string createViewSql = $"CREATE VIEW {viewName} AS {viewDefinitionSql}";
                _logService.Debug($"Executando CREATE VIEW para: {viewName}. SQL: {createViewSql}");
                using (var createCommand = new SqlCommand(createViewSql, connection))
                {
                    await createCommand.ExecuteNonQueryAsync();
                }
                _logService.Info($"View '{viewName}' criada com sucesso.");
            }

            _logService.Info($"View '{viewName}' criada/alterada com sucesso.");
            return Result.Success();  
        }
        catch (SqlException ex)
        {
            _logService.Error(ex, $"Erro SQL ao criar/alterar view '{viewName}'. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}. " +
                                  $"Código do Erro SQL: {ex.Number}. Mensagem: {ex.Message}. " +
                                  $"SQL Original: '{viewDefinitionSql}'"); // Adicionada viewDefinitionSql para debugging
            return Result.Failure($"Erro de banco de dados ao criar/alterar view '{viewName}' (Código SQL: {ex.Number}): {ex.Message}");
        }
        catch (Exception ex)
        {
            _logService.Error(ex, $"Erro inesperado ao criar/alterar view '{viewName}'. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}. " +
                                  $"Mensagem: {ex.Message}.");
            return Result.Failure($"Erro inesperado ao criar/alterar view '{viewName}': {ex.Message}");
        }
    }

    /// <summary>
    /// Executa um comando SQL arbitrário no banco de dados usando os parâmetros de conexão fornecidos.
    /// </summary>
    /// <param name="parameters">Parâmetros que contêm informações da conexão, como servidor, banco de dados, usuário e senha.</param>
    /// <param name="sqlCommand">Comando SQL a ser executado. Deve ser uma instrução válida, como CREATE, ALTER, DROP ou INSERT.</param>
    /// <returns>Uma tarefa que representa a execução assíncrona do comando SQL.</returns>
    /// <exception cref="SqlException">Lançada se ocorrer um erro ao executar o comando no SQL Server.</exception>
    public async Task ExecuteSqlCommandAsync(ConnectionParameters parameters, string sqlCommand)
    {
        string connectionString = Helpers.SqlConnectionStringBuilderHelper.Build(parameters, true);
        using (var connection = new SqlConnection(connectionString))
        {
            _logService.Debug($"Abrindo conexão para executar comando: {sqlCommand}");
            await connection.OpenAsync();
            using (var command = new SqlCommand(sqlCommand, connection))
            {
                await command.ExecuteNonQueryAsync();
                _logService.Debug($"Comando executado com sucesso: {sqlCommand}");
            }
        }
    }
}