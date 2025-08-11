using CSharpFunctionalExtensions;
using Exportador.Application.DTOs;
using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;

namespace Exportador.Application.UseCases;

/// <summary>
/// Caso de uso responsável por buscar os nomes de tabelas no banco de dados.
/// </summary>
public class GetTableNames
{
    private readonly ITableDiscoveryService _tableDiscoveryService;
    private readonly ILogService _logService;

    /// <summary>
    /// Construtor do caso de uso, injetando o serviço de descoberta de tabelas e o serviço de log.
    /// </summary>
    /// <param name="tableDiscoveryService">O serviço de infraestrutura para buscar nomes de tabelas.</param>
    /// <param name="logService">O serviço de log para registrar informações e erros.</param>
    /// <exception cref="ArgumentNullException">Lançada se os serviços injetados forem nulos.</exception>
    public GetTableNames(ITableDiscoveryService tableDiscoveryService, ILogService logService)
    {
        _tableDiscoveryService = tableDiscoveryService ?? throw new ArgumentNullException(nameof(tableDiscoveryService));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    /// <summary>
    /// Executa a busca pelos nomes das tabelas do banco de dados.
    /// </summary>
    /// <param name="connectionParametersDto">DTO com os parâmetros de conexão.</param>
    /// <param name="searchPattern">Padrão SQL LIKE para filtrar os nomes das tabelas.</param>
    /// <returns>Um Result contendo uma lista de nomes de tabelas se a operação for bem-sucedida,
    /// ou uma mensagem de erro em caso de falha.</returns>
    public async Task<Result<List<string>>> ExecuteAsync(ConnectionParametersDto connectionParametersDto, string searchPattern)
    {
        _logService.Info($"Iniciando caso de uso 'GetTableNames.ExecuteAsync' com searchPattern: '{searchPattern}'");
         
        var connectionParamsResult = ConnectionParameters.Create(
            connectionParametersDto.Server,
            connectionParametersDto.Database,
            connectionParametersDto.User,
            connectionParametersDto.Password!
        );

        if (connectionParamsResult.IsFailure)
        { 
            _logService.Warning($"Validação de entrada falhou no caso de uso 'GetTableNames': {connectionParamsResult.Error}");
            return Result.Failure<List<string>>(connectionParamsResult.Error);
        }

        var connectionParameters = connectionParamsResult.Value;

        try
        { 
            var result = await _tableDiscoveryService.GetTableNamesAsync(connectionParameters, searchPattern);
             
            if (result.IsSuccess)
            {
                _logService.Info($"Descoberta de tabelas concluída com sucesso. {result.Value.Count} tabelas encontradas.");
                return result;  
            }
            else
            {
                _logService.Warning($"Descoberta de tabelas falhou: {result.Error}");
                return result;  
            }
        }
        catch (Exception ex)
        {
            _logService.Error(ex, "Um erro inesperado ocorreu durante a execução do caso de uso de busca de tabelas.");
            return Result.Failure<List<string>>($"Um erro inesperado ocorreu: {ex.Message}");
        }
    }
}