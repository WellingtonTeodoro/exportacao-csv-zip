using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;

namespace Exportador.Application.UseCases.Steps;


/// <summary>
/// Representa o passo de configuração e verificação das views de banco de dados necessárias para a exportação.
/// Esta etapa é crucial para garantir que as fontes de dados estejam prontas antes da extração.
/// </summary>
public class SetupDatabaseViewsStep : IExportStep
{
    /// <summary>
    /// O serviço responsável por configurar o esquema do banco de dados (ex: criar ou atualizar views).
    /// </summary>
    private readonly IDatabaseSetupService _dbSetupService;

    /// <summary>
    /// O serviço de log para registrar o andamento da operação.
    /// </summary>
    private readonly ILogService _log;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="SetupDatabaseViewsStep"/>.
    /// </summary>
    /// <param name="dbSetupService">A instância do serviço de configuração de banco de dados.</param>
    /// <param name="log">A instância do serviço de log.</param>
    public SetupDatabaseViewsStep(IDatabaseSetupService dbSetupService, ILogService log)
    {
        _dbSetupService = dbSetupService ?? throw new ArgumentNullException(nameof(dbSetupService));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    /// <summary>
    /// Executa a rotina de configuração das views no banco de dados.
    /// </summary>
    /// <param name="context">O contexto da exportação, que fornece os parâmetros de conexão.</param>
    /// <returns>Um <see cref="Result"/> indicando o sucesso ou a falha da configuração.</returns>
    public async Task<Result> ExecuteAsync(ExportContext context)
    {
        context.ProgressReporter.Report(new ExportStatus(5, "Configurando e verificando views de exportação..."));
        _log.Info("Iniciando a configuração das views de exportação.");

        var setupResult = await _dbSetupService.SetupRequiredViewsAsync((Core.ValueObjects.ConnectionParameters)context.ConnectionParameters!);
        if (setupResult.IsFailure)
        {
            var errorMessage = $"Erro ao configurar o banco de dados (Views): {setupResult.Error}";
            _log.Error($"Falha ao configurar as views obrigatórias: {setupResult.Error}");
            return Result.Failure(errorMessage);
        }

        _log.Info("Views de exportação verificadas e configuradas com sucesso.");
        context.ProgressReporter.Report(new ExportStatus(10, "Views configuradas com sucesso."));

        return Result.Success();
    }
}