using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;
using Exportador.Infrastructure.Helpers;  
using Microsoft.Data.SqlClient;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Implementação concreta de <see cref="IDatabaseConnectionService"/> para SQL Server.
/// Lida com os detalhes de baixo nível da conexão ao banco de dados.
/// </summary>
public class ConnectionTestService : IDatabaseConnectionService
{
    private readonly ILogService _logService;  

    /// <summary>
    /// Construtor de <see cref="ConnectionTestService"/>.
    /// </summary>
    /// <param name="logService">Serviço de log para registrar informações e erros.</param>
    public ConnectionTestService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("ConnectionTestService inicializado.");
    }

    /// <summary>
    /// Testa a conexão com um banco de dados SQL Server utilizando os parâmetros fornecidos.
    /// </summary>
    /// <param name="parameters">Parâmetros de conexão, incluindo servidor, banco de dados, usuário e senha.</param>
    /// <returns>
    /// Um objeto <see cref="ConnectionTestResult"/> indicando sucesso ou falha da tentativa de conexão.
    /// </returns>
    /// <remarks>
    /// O parâmetro <c>TrustServerCertificate=True</c> é utilizado para evitar problemas com certificados durante o desenvolvimento.
    /// Em ambientes de produção, recomenda-se configurar certificados de forma apropriada.
    /// </remarks>
    public async Task<ConnectionTestResult> TestConnectionAsync(ConnectionParameters parameters)
    { 
        if (parameters == null)
        {
            _logService.Error("Tentativa de testar conexão com parâmetros nulos.");
            return ConnectionTestResult.Fail("Parâmetros de conexão não podem ser nulos.");
        }
         
        try
        { 
            var connectionString = SqlConnectionStringBuilderHelper.Build(parameters, trustServerCertificate: true);

            _logService.Debug($"Tentando conexão com Server: {parameters.Server}, Database: {parameters.Database}, User: {parameters.User}.");

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                _logService.Info($"Conexão com o banco de dados '{parameters.Database}' no servidor '{parameters.Server}' testada com sucesso.");
                return ConnectionTestResult.Success();
            }
        }
        catch (SqlException ex)
        { 
            _logService.Error(ex, $"SqlException ao testar conexão. " +
                                  $"Servidor: {parameters.Server}, Banco: {parameters.Database}, Usuário: {parameters.User}. " +
                                  $"Código do Erro SQL: {ex.Number}. Mensagem: {ex.Message}");
            return ConnectionTestResult.Fail($"Erro de conexão SQL: {ex.Message} (Código: {ex.Number})");
        }
        catch (Exception ex)
        { 
            _logService.Error(ex, $"Erro inesperado ao testar conexão. " +
                                  $"Servidor: {parameters.Server}, Banco: {parameters.Database}, Usuário: {parameters.User}. " +
                                  $"Mensagem: {ex.Message}");
            return ConnectionTestResult.Fail($"Erro inesperado ao testar conexão: {ex.Message}");
        }
    } 
}