using Exportador.Application.Configuration;
using Exportador.Application.Interfaces;
using Exportador.Application.UseCases;
using Exportador.Application.UseCases.Steps;
using Exportador.Infrastructure.Repositories; 

namespace Exportador.Infrastructure.Factories;

/// <summary>
/// Fábrica responsável por criar o contexto e os passos necessários para o processo de exportação.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IExportContextFactory"/>, centralizando a criação do <see cref="ExportContext"/>
/// e dos passos (steps) que compõem o fluxo de exportação.
/// </remarks>
public class ExportContextFactory : IExportContextFactory
{
    private readonly IConnectionParametersStore _connectionParametersStore;
    private readonly IDatabaseSetupService _databaseSetupService;
    private readonly IDataRepositoryService _dataRepositoryService;
    private readonly IDataRetrievalService _dataRetrievalService;
    private readonly ICsvWriterService _csvWriterService;
    private readonly IExportSettingsRepository _exportSettingsRepository;
    private readonly IZipService _zipService;
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância da fábrica de contexto de exportação com os serviços necessários.
    /// </summary>
    /// <param name="connectionParametersStore">Armazenamento dos parâmetros de conexão do banco.</param>
    /// <param name="databaseSetupService">Serviço para preparação do banco (views, etc.).</param>
    /// <param name="dataRepositoryService">Serviço para obtenção dos dados e metadados das tabelas.</param>
    /// <param name="dataRetrievalService">Serviço para recuperação dos dados efetivos para exportação.</param>
    /// <param name="csvWriterService">Serviço para escrita dos dados em arquivos CSV.</param>
    /// <param name="exportSettingsRepository">Repositório das configurações de exportação.</param>
    /// <param name="zipService">Serviço responsável pela criação de arquivos ZIP.</param>
    /// <param name="logService">Serviço de logging para auditoria e rastreamento.</param>
    public ExportContextFactory(
        IConnectionParametersStore connectionParametersStore,
        IDatabaseSetupService databaseSetupService,
        IDataRepositoryService dataRepositoryService,
        IDataRetrievalService dataRetrievalService,
        ICsvWriterService csvWriterService,
        IExportSettingsRepository exportSettingsRepository,
        IZipService zipService,
        ILogService logService)
    {
        _connectionParametersStore = connectionParametersStore;
        _databaseSetupService = databaseSetupService;
        _dataRepositoryService = dataRepositoryService;
        _dataRetrievalService = dataRetrievalService;
        _csvWriterService = csvWriterService;
        _exportSettingsRepository = exportSettingsRepository;
        _zipService = zipService;
        _logService = logService;
    }

    /// <summary>
    /// Cria o contexto de exportação, que mantém o estado e reporta progresso durante a execução.
    /// </summary>
    /// <returns>Instância de <see cref="ExportContext"/> inicializada.</returns>
    public ExportContext CreateContext(ExportData exportData)
    { 
        var progressReporter = new ExportData.LegacyProgressReporter(exportData);
        return new ExportContext(progressReporter);
    }

    /// <summary>
    /// Cria a sequência de passos (steps) que serão executados para realizar a exportação dos dados.
    /// </summary>
    /// <returns>Uma coleção de instâncias que implementam <see cref="IExportStep"/> representando o fluxo de exportação.</returns>
    public IEnumerable<IExportStep> CreateSteps()
    {
        return new List<IExportStep>
        {
            new ValidateConnectionStep(_connectionParametersStore, _logService),
            new SetupDatabaseViewsStep(_databaseSetupService, _logService),
            new GetSelectedTablesStep(_dataRepositoryService, _logService),
            new ProcessTablesStep(_dataRetrievalService, _csvWriterService,
            new InMemoryExportMappingRepository(),
            new DefaultFileSystemService(), _logService),
            new CreateZipArchiveStep(_zipService, _exportSettingsRepository,
            new DefaultFileSystemService(), _logService)
        };
    }

    /// <summary>
    /// Cria o passo responsável pela limpeza dos arquivos temporários gerados durante o processo de exportação.
    /// </summary>
    /// <returns>Instância de <see cref="IExportStep"/> para limpeza de arquivos temporários.</returns>
    public IExportStep CreateCleanupStep()
    {
        return new CleanupTemporaryFilesStep(new DefaultFileSystemService(), _logService);
    }
}
