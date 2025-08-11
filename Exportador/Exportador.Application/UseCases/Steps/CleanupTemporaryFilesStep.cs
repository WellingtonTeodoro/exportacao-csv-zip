using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;

namespace Exportador.Application.UseCases.Steps;

/// <summary>
/// Representa o passo final de limpeza dos arquivos temporários gerados durante a exportação.
/// </summary>
public class CleanupTemporaryFilesStep : IExportStep
{
    /// <summary>
    /// O serviço que abstrai as operações do sistema de arquivos.
    /// </summary>
    private readonly IFileSystemService _fileSystem;

    /// <summary>
    /// O serviço de log para registrar o andamento.
    /// </summary>
    private readonly ILogService _log;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="CleanupTemporaryFilesStep"/>.
    /// </summary>
    /// <param name="fileSystem">A instância do serviço de sistema de arquivos.</param>
    /// <param name="log">A instância do serviço de log.</param>
    public CleanupTemporaryFilesStep(IFileSystemService fileSystem, ILogService log)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    /// <summary>
    /// Executa a exclusão de todos os arquivos CSV temporários listados no contexto.
    /// </summary>
    /// <param name="context">O contexto da exportação, que contém os caminhos dos arquivos a serem limpos.</param>
    /// <returns>Um <see cref="Result"/> que sempre será de sucesso, pois falhas na limpeza não devem falhar a exportação como um todo.</returns>
    public Task<Result> ExecuteAsync(ExportContext context)
    {
        context.ProgressReporter.Report(new ExportStatus(98, "Finalizando e limpando arquivos temporários..."));
        _log.Info("Iniciando limpeza de arquivos temporários.");

        if (!context.TemporaryCsvFilePaths.Any())
        {
            _log.Info("Nenhum arquivo temporário para limpar.");
            return Task.FromResult(Result.Success());
        }

        foreach (var filePath in context.TemporaryCsvFilePaths)
        {
            try
            {
                _fileSystem.DeleteFile(filePath);
                _log.Debug($"Arquivo temporário '{filePath}' excluído com sucesso.");
            }
            catch (Exception ex)
            { 
                _log.Warning($"Não foi possível excluir o arquivo temporário '{filePath}': {ex.Message}");
            }
        }

        context.TemporaryCsvFilePaths.Clear();
        _log.Info("Limpeza de arquivos temporários concluída.");

        return Task.FromResult(Result.Success());
    }
}