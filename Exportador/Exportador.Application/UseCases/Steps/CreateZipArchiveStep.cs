using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;

namespace Exportador.Application.UseCases.Steps;

/// <summary>
/// Representa o passo de criação do arquivo ZIP contendo todos os CSVs gerados.
/// </summary>
public class CreateZipArchiveStep : IExportStep
{
    private readonly IZipService _zipService;
    private readonly IExportSettingsRepository _settingsRepo;
    private readonly IFileSystemService _fileSystem;
    private readonly ILogService _log;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="CreateZipArchiveStep"/>.
    /// </summary>
    public CreateZipArchiveStep(IZipService zipService, IExportSettingsRepository settingsRepo, IFileSystemService fileSystem, ILogService log)
    {
        _zipService = zipService ?? throw new ArgumentNullException(nameof(zipService));
        _settingsRepo = settingsRepo ?? throw new ArgumentNullException(nameof(settingsRepo));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    /// <summary>
    /// Executa a compactação dos arquivos CSV temporários.
    /// </summary>
    /// <param name="context">O contexto da exportação, que contém os caminhos dos arquivos temporários.</param>
    /// <returns>Um <see cref="Result"/> indicando o sucesso ou a falha da criação do ZIP.</returns>
    public Task<Result> ExecuteAsync(ExportContext context)
    {
        context.ProgressReporter.Report(new ExportStatus(85, "Compactando arquivos..."));

        if (!context.TemporaryCsvFilePaths.Any())
        {
            _log.Warning("Nenhum arquivo CSV para compactar. Pulando a etapa de criação do ZIP.");
            return Task.FromResult(Result.Success());
        }

        try
        {
            var destinationDirectory = _settingsRepo.GetDestinationDirectoryPath();
            var outputFileName = _settingsRepo.GetOutputFileName();

            if (!_fileSystem.DirectoryExists(destinationDirectory))
            {
                _fileSystem.CreateDirectory(destinationDirectory);
                _log.Info($"Diretório de destino '{destinationDirectory}' foi criado.");
            }

            context.FinalZipFilePath = _fileSystem.Combine(destinationDirectory, $"{outputFileName}.zip");
            _zipService.CreateZipFile(context.FinalZipFilePath, context.TemporaryCsvFilePaths);

            _log.Info($"Arquivo ZIP '{context.FinalZipFilePath}' criado com sucesso.");
            context.ProgressReporter.Report(new ExportStatus(95, "Arquivo ZIP criado com sucesso."));

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao criar arquivo ZIP: {ex.Message}";
            _log.Error(ex, errorMessage);
            return Task.FromResult(Result.Failure(errorMessage));
        }
    }
}