using Exportador.Application.DTOs;
using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;

namespace Exportador.Application.UseCases;

/// <summary>
/// Caso de uso responsável por coordenar o teste de conexão com o banco de dados.
/// Delega a execução para o serviço de infraestrutura e registra eventos.
/// </summary>
public class TestDatabaseConnection
{
    private readonly IDatabaseConnectionService _connectionService;
    private readonly ILogService _logService;

    /// <summary>
    /// Construtor do caso de uso, injetando o serviço de conexão e o serviço de log.
    /// </summary>
    /// <param name="connectionService">O serviço de infraestrutura para testar a conexão.</param>
    /// <param name="logService">O serviço de log para registrar informações e erros.</param>
    /// <exception cref="ArgumentNullException">Lançada se os serviços injetados forem nulos.</exception>
    public TestDatabaseConnection(IDatabaseConnectionService connectionService, ILogService logService)
    {
        _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    /// <summary>
    /// Executa o teste de conexão do banco de dados.
    /// </summary>
    /// <param name="parametersDto">Um DTO contendo os parâmetros de conexão fornecidos pela UI.</param>
    /// <returns>Um Objeto de Valor <see cref="ConnectionTestResult"/> com o resultado do teste (sucesso/falha e mensagem de erro).</returns>
    public async Task<ConnectionTestResult> ExecuteAsync(ConnectionParametersDto parametersDto)
    {
        _logService.Info($"Iniciando caso de uso 'TestDatabaseConnection.ExecuteAsync' com DTO recebido: " +
                  $"Server='{parametersDto.Server}', DB='{parametersDto.Database}', User='{parametersDto.User}', " +
                  $"PasswordPresent={!(parametersDto.Password == null || parametersDto.Password.Length == 0)}");
         
        var connectionParamsResult = ConnectionParameters.Create(
            parametersDto.Server,
            parametersDto.Database,
            parametersDto.User,
            parametersDto.Password!  
        );

        if (connectionParamsResult.IsFailure)
        { 
            _logService.Warning($"Validação de entrada falhou no caso de uso 'TestDatabaseConnection': {connectionParamsResult.Error}");
            return ConnectionTestResult.Fail(connectionParamsResult.Error);
        }

        var connectionParameters = connectionParamsResult.Value;

        try
        { 
            var result = await _connectionService.TestConnectionAsync(connectionParameters);

            if (result.IsSuccess)
            {
                _logService.Info("Teste de conexão com o banco de dados concluído com sucesso.");
                return ConnectionTestResult.Success();
            }
            else
            { 
                _logService.Warning($"Teste de conexão com o banco de dados falhou: {result.ErrorMessage}");
                return ConnectionTestResult.Fail(result.ErrorMessage!);
            }
        }
        catch (Exception ex)
        { 
            _logService.Error(ex, $"Um erro inesperado ocorreu durante a execução do caso de uso de teste de conexão: {ex.Message}"); 
            return ConnectionTestResult.Fail($"Um erro inesperado ocorreu: {ex.Message}");
        }
    }
}