using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;

namespace Exportador.Application.UseCases.Steps;

/// <summary>
/// Passo que processa as tabelas selecionadas, recuperando dados e gerando arquivos CSV.
/// </summary>
public class ProcessTablesStep : IExportStep
{
    private readonly IDataRetrievalService _dataRetrieval;
    private readonly ICsvWriterService _csvWriter;
    private readonly IExportMappingRepository _mappingRepo;
    private readonly IFileSystemService _fileSystem;
    private readonly ILogService _log;

    /// <summary>
    /// Inicializa a instância do passo com os serviços necessários.
    /// </summary>
    public ProcessTablesStep(
        IDataRetrievalService dataRetrieval,
        ICsvWriterService csvWriter,
        IExportMappingRepository mappingRepo,
        IFileSystemService fileSystem,
        ILogService log)
    {
        _dataRetrieval = dataRetrieval ?? throw new ArgumentNullException(nameof(dataRetrieval));
        _csvWriter = csvWriter ?? throw new ArgumentNullException(nameof(csvWriter));
        _mappingRepo = mappingRepo ?? throw new ArgumentNullException(nameof(mappingRepo));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    /// <summary>
    /// Executa o processamento das tabelas, gerando CSV e atualizando o contexto.
    /// </summary>
    /// <param name="context">Contexto da exportação com as informações e estado compartilhado.</param>
    /// <returns>Resultado indicando sucesso ou falha.</returns>
    public async Task<Result> ExecuteAsync(ExportContext context)
    {
        const int startProgress = 10;
        const int endProgress = 80;
        double progressRange = endProgress - startProgress;
        var tablesToProcess = context.SelectedFriendlyTableNames;

        if (!tablesToProcess.Any())
            return Result.Success();

        double progressPerTable = progressRange / tablesToProcess.Count;

        for (int i = 0; i < tablesToProcess.Count; i++)
        {
            var friendlyName = tablesToProcess[i];
            int currentProgress = startProgress + (int)(i * progressPerTable);
            context.ProgressReporter?.Report(new ExportStatus(currentProgress, $"Processando: {friendlyName}..."));

            var mappingResult = _mappingRepo.GetMapping(friendlyName);
            if (mappingResult.HasNoValue)
            {
                _log.Warning($"Mapeamento não encontrado para '{friendlyName}'. Ignorando esta tabela.");
                continue;
            }

            var (realTableName, selectColumns) = mappingResult.Value;

            try
            {
                var dataResult = await _dataRetrieval
                    .GetDataFromTableAsync((Exportador.Core.ValueObjects.ConnectionParameters)context.ConnectionParameters!, realTableName, selectColumns);

                if (dataResult.IsFailure)
                {
                    _log.Warning($"Erro ao recuperar dados da tabela '{realTableName}': {dataResult.Error}");
                    context.ProgressReporter?.Report(new ExportStatus(currentProgress, $"⚠️ Tabela '{friendlyName}' não encontrada ou inválida. Ignorada."));
                    continue;
                }

                var tableData = dataResult.Value;
                 
                context.SqlRecordsCountPerTable[friendlyName] = tableData.Count;

                string tempCsvFilePath = _fileSystem.Combine(_fileSystem.GetTempPath(), $"{friendlyName}.csv");

                await _csvWriter.WriteCsvAsync(tempCsvFilePath, tableData);
                context.TemporaryCsvFilePaths.Add(tempCsvFilePath);

                _log.Info($"Arquivo CSV para '{friendlyName}' criado em '{tempCsvFilePath}'. {tableData.Count} registros gravados.");
            }
            catch (Exception ex)
            {
                _log.Error($"Exceção ao processar '{friendlyName}': {ex.Message}", ex);
                context.ProgressReporter?.Report(new ExportStatus(currentProgress, $"Erro inesperado ao processar '{friendlyName}'. Verifique os logs."));
                continue;
            }
        }

        context.ProgressReporter?.Report(new ExportStatus(endProgress, "Geração de arquivos CSV concluída."));
        return Result.Success();
    }
}