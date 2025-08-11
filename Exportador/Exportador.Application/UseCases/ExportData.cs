using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;
using Exportador.Core.Models;
using System.Diagnostics;
using System.IO;

namespace Exportador.Application.UseCases;

/// <summary>
/// Caso de uso principal que orquestra todo o processo de exportação dos dados.
/// </summary>
public class ExportData : IExportDataUseCase
{
    private readonly SolidExportOrchestrator _orchestrator;
    private readonly IExportContextFactory _contextFactory;
    private readonly IExportResultRepository _exportResultRepository;

    /// <summary>
    /// Evento disparado para atualização percentual do progresso da exportação.
    /// </summary>
    public event EventHandler<int>? ProgressUpdated;

    /// <summary>
    /// Evento disparado para atualização de mensagens de status da exportação.
    /// </summary>
    public event EventHandler<string>? StatusMessageUpdated;

    /// <summary>
    /// Inicializa uma nova instância do caso de uso.
    /// </summary>
    /// <param name="orchestrator">Orquestrador que executa os passos da exportação.</param>
    /// <param name="contextFactory">Fábrica que cria o contexto e passos para a exportação.</param>
    /// <param name="exportResultRepository">Repositório para salvar os dados do resultado da exportação.</param>
    public ExportData(
        SolidExportOrchestrator orchestrator,
        IExportContextFactory contextFactory,
        IExportResultRepository exportResultRepository)
    {
        _orchestrator = orchestrator ?? throw new ArgumentNullException(nameof(orchestrator));
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _exportResultRepository = exportResultRepository ?? throw new ArgumentNullException(nameof(exportResultRepository)); 
    } 

    /// <summary>
    /// Executa o processo completo de exportação, incluindo limpeza e salvamento do resultado.
    /// </summary>
    /// <returns>Resultado da operação.</returns>
    public async Task<Result> ExecuteExportAsync()
    {
        var context = _contextFactory.CreateContext(this);
        var steps = _contextFactory.CreateSteps();
        var cleanupStep = _contextFactory.CreateCleanupStep();

        _orchestrator.SetSteps(steps);

        var stopwatch = Stopwatch.StartNew();

        Result result;
        try
        {
            result = await _orchestrator.ExecuteAsync(context);
        }
        finally
        {
            await cleanupStep.ExecuteAsync(context);
        }

        stopwatch.Stop();

        if (result.IsSuccess)
        {
            await SaveExportResultAsync(context, stopwatch.Elapsed);
        }

        return result;
    }

    /// <summary>
    /// Salva as informações do resultado da exportação no repositório.
    /// </summary>
    /// <param name="context">Contexto da exportação com dados necessários.</param>
    /// <param name="elapsed">Tempo total da exportação.</param>
    /// <returns>Tarefa assíncrona.</returns>
    private async Task SaveExportResultAsync(ExportContext context, TimeSpan elapsed)
    {
        var zipFilePath = context.FinalZipFilePath;

        var exportResult = new ExportResultInfo
        {
            ZipFileName = zipFilePath != null ? Path.GetFileName(zipFilePath) : "N/A",
            ZipFileSizeBytes = zipFilePath != null && File.Exists(zipFilePath) ? new FileInfo(zipFilePath).Length : 0,
            TotalExportTime = elapsed,
            ExportedTables = context.SqlRecordsCountPerTable
                .Select(kvp => new ExportedTableInfo
                {
                    TableName = kvp.Key,
                    SqlRecordsCount = kvp.Value
                })
                .ToList()
        };

        await _exportResultRepository.SaveAsync(exportResult);
    }

    /// <summary>
    /// Método de fábrica para criar reporter com this
    /// </summary>
    /// <returns></returns>
    public IProgressReporter CreateLegacyProgressReporter()
        => new LegacyProgressReporter(this);

    /// <summary>
    /// Implementação legada de <see cref="IProgressReporter"/> para integração com eventos.
    /// </summary>
    public class LegacyProgressReporter : IProgressReporter
    {
        private readonly ExportData _exportDataInstance;

        /// <summary>
        /// Última porcentagem reportada.
        /// </summary>
        public int LastPercentage { get; private set; }

        /// <summary>
        /// Inicializa a instância do reporter legado.
        /// </summary>
        /// <param name="exportDataInstance">Instância do caso de uso ExportData para envio de eventos.</param>
        public LegacyProgressReporter(ExportData exportDataInstance)
        {
            _exportDataInstance = exportDataInstance ?? throw new ArgumentNullException(nameof(exportDataInstance));
        }

        /// <summary>
        /// Reporta progresso atual e dispara eventos.
        /// </summary>
        /// <param name="status">Status atual com progresso e mensagem.</param>
        public void Report(ExportStatus status)
        {
            LastPercentage = status.ProgressPercentage;

            _exportDataInstance?.OnStatusMessageUpdated(status.Message);
            _exportDataInstance?.OnProgressUpdated(status.ProgressPercentage);
        } 

    }

    /// <summary>
    /// Dispara evento de atualização de progresso.
    /// </summary>
    /// <param name="progress">Porcentagem do progresso.</param>
    protected virtual void OnProgressUpdated(int progress)
        => ProgressUpdated?.Invoke(this, progress);

    /// <summary>
    /// Dispara evento de atualização de mensagem de status.
    /// </summary>
    /// <param name="message">Mensagem de status.</param>
    protected virtual void OnStatusMessageUpdated(string message)
        => StatusMessageUpdated?.Invoke(this, message);
}