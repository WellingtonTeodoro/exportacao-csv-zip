using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;  
using Microsoft.Data.SqlClient;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Implementação de <see cref="IDatabaseConnectionService"/> para testar a conectividade
/// com bancos de dados SQL Server.
/// </summary>
public class SqlDatabaseConnectionService : IDatabaseConnectionService
{
    private readonly ILogService _logService;

    /// <summary>
    /// Construtor de <see cref="SqlDatabaseConnectionService"/>.
    /// </summary>
    /// <param name="logService">Serviço de log para registrar informações e erros.</param>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="logService"/> for nulo.</exception>
    public SqlDatabaseConnectionService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("SqlDatabaseConnectionService inicializado.");
    }

    /// <summary>
    /// Testa a conexão com um banco de dados SQL Server utilizando os parâmetros fornecidos.
    /// </summary>
    /// <param name="connectionParameters">Objeto de valor com os parâmetros da conexão. Não pode ser nulo.</param>
    /// <returns>Um Objeto de Valor <see cref="ConnectionTestResult"/> com o resultado do teste.</returns>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="connectionParameters"/> for nulo.</exception>
    public async Task<ConnectionTestResult> TestConnectionAsync(ConnectionParameters connectionParameters)
    {
        if (connectionParameters == null)
        {
            const string errorMessage = "Parâmetros de conexão não podem ser nulos para testar a conexão.";
            _logService.Error(errorMessage); 
            return ConnectionTestResult.Fail(errorMessage);
        }
         
        string currentConnectionString = Helpers.SqlConnectionStringBuilderHelper.Build(connectionParameters, true);

        try
        {
            using (var connection = new SqlConnection(currentConnectionString))
            {
                await connection.OpenAsync();
                _logService.Info("Conexão com o banco de dados SQL Server estabelecida com sucesso."); 
                return ConnectionTestResult.Success();
            }
        }
        catch (SqlException ex)
        { 
            _logService.Error(ex, $"Falha na conexão SQL. Detalhes: {ex.Message}. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}.");
            return ConnectionTestResult.Fail($"Falha na conexão: {ex.Message}");
        }
        catch (Exception ex)
        { 
            _logService.Error(ex, $"Erro inesperado ao testar a conexão com o banco de dados: {ex.Message}. " +
                                  $"Servidor: {connectionParameters.Server}, Banco: {connectionParameters.Database}.");
            return ConnectionTestResult.Fail($"Erro inesperado: {ex.Message}");
        }
    }
}