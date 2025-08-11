using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;
using Exportador.Application.Models;
using Exportador.Core.ValueObjects;
using Microsoft.Data.SqlClient;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Implementação concreta de <see cref="IDatabaseSetupService"/> para configurar
/// o esquema do banco de dados SQL Server, focando na criação e atualização de views.
/// </summary>
public class DatabaseSetupService : IDatabaseSetupService
{
    private readonly ISchemaManagementService _schemaManagementService;
    private readonly ILogService _logService;
    private readonly IReadOnlyList<DatabaseViewDefinition> _requiredViews;

    /// <summary>
    /// Inicializa uma nova instância do serviço <see cref="DatabaseSetupService"/>.
    /// </summary>
    /// <param name="schemaManagementService">Serviço para executar operações de esquema no banco de dados.</param>
    /// <param name="logService">Serviço de log para registrar informações e erros.</param>
    /// <param name="viewDefinitionProvider">Serviço injetado para prover as definições das views.</param>
    /// <remarks>
    /// As definições de views são carregadas via o provedor injetado (<see cref="IViewDefinitionProvider"/>).
    /// </remarks>
    /// <exception cref="ArgumentNullException">Lançada se qualquer um dos serviços injetados for nulo.</exception>
    public DatabaseSetupService(ISchemaManagementService schemaManagementService, 
        ILogService logService, IViewDefinitionProvider viewDefinitionProvider)
    {
        _schemaManagementService = schemaManagementService ?? throw new ArgumentNullException(nameof(schemaManagementService));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _requiredViews = viewDefinitionProvider.GetRequiredViews();  
        _logService.Info("DatabaseSetupService inicializado."); 
    }

    /// <summary>
    /// Configura (cria ou atualiza) todas as views obrigatórias no banco de dados
    /// usando os parâmetros de conexão fornecidos.
    /// </summary>
    /// <param name="connectionParameters">Os parâmetros de conexão com o banco de dados.</param>
    /// <returns>
    /// Um <see cref="Result"/> indicando sucesso ou falha.
    /// Retorna <see cref="Result.Failure"/> com uma mensagem de erro em caso de falha,
    /// ou <see cref="Result.Success"/> em caso de sucesso.
    /// </returns>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="connectionParameters"/> for nulo.</exception>
    public async Task<Result> SetupRequiredViewsAsync(ConnectionParameters connectionParameters)
    {
        if (connectionParameters == null)
        {
            _logService.Error("Parâmetros de conexão nulos fornecidos para SetupRequiredViewsAsync.");
            return Result.Failure("Parâmetros de conexão são obrigatórios para configurar as views.");
        }

        _logService.Info("Iniciando configuração e verificação das views obrigatórias no banco de dados.");

        foreach (var view in _requiredViews)
        {
            _logService.Debug($"Tentando criar/alterar view: {view.ViewName}");
            try
            { 
                await _schemaManagementService.CreateOrAlterViewAsync(connectionParameters, view.ViewName, view.ViewSqlDefinition);
                _logService.Info($"View '{view.ViewName}' configurada com sucesso.");

                await _schemaManagementService.ExecuteSqlCommandAsync(connectionParameters, $"EXEC sp_refreshview '{view.ViewName}'");
                _logService.Debug($"sp_refreshview executado para: {view.ViewName}");
            }
            catch (SqlException ex)
            { 
                _logService.Error(ex, $"Falha de SQL ao configurar a view '{view.ViewName}'. " +
                                      $"Código do Erro SQL: {ex.Number}. Mensagem: {ex.Message}");
                return Result.Failure($"Erro de banco de dados ao configurar view '{view.ViewName}' (Código SQL: {ex.Number}): {ex.Message}");
            }
            catch (Exception ex)
            { 
                _logService.Error(ex, $"Falha crítica inesperada ao configurar a view '{view.ViewName}'. A exportação pode não ser possível. Mensagem: {ex.Message}");
                return Result.Failure($"Erro inesperado ao configurar a view '{view.ViewName}': {ex.Message}");
            }
        }

        _logService.Info("Todas as views obrigatórias foram configuradas com sucesso.");
        await Task.Delay(2000);
        return Result.Success();
    }
}