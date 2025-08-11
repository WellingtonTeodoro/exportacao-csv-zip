using Exportador.Application.Configuration;
using Exportador.Application.Interfaces;
using Exportador.Application.UseCases;
using Exportador.Application.UseCases.Steps;
using Exportador.Infrastructure.Configuration;
using Exportador.Infrastructure.Factories;
using Exportador.Infrastructure.Logging;
using Exportador.Infrastructure.Repositories;
using Exportador.Infrastructure.Services;
using Exportador.UI.Interfaces;
using Exportador.UI.Repositories;
using Exportador.UI.Services;
using Exportador.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Windows;
using System.IO; 

namespace Exportador.UI;
/// <summary>
/// Classe principal da aplicação WPF.
/// Responsável pela inicialização da aplicação, configuração de serviços e gerenciamento do ciclo de vida da aplicação.
/// </summary>
public partial class App : System.Windows.Application
{
    /// <summary>
    /// Container de injeção de dependência usado para resolver serviços em tempo de execução.
    /// </summary>
    private ServiceProvider? _serviceProvider;

    /// <summary>
    /// Construtor padrão da aplicação. Pode ser usado para inicializações prévias.
    /// </summary>
    public App()
    { 
    }

    /// <summary>
    /// Evento disparado quando a aplicação é iniciada.
    /// Responsável por configurar o Serilog, DI e mostrar a janela principal.
    /// </summary>
    /// <param name="e">Argumentos do evento de inicialização.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.File("logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Log.Information("Aplicação Exportador.UI iniciada.");

        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    } 

    /// <summary>
    /// Registra todos os serviços e componentes necessários da aplicação.
    /// Utiliza o padrão Dependency Injection para gerenciamento de instâncias.
    /// </summary>
    /// <param name="services">Coleção de serviços a serem configurados.</param>
    private void ConfigureServices(IServiceCollection services)
    {
        // ------------------------------------------------------------------------------
        // Serviços de logging e núcleo da aplicação
        // ------------------------------------------------------------------------------
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<ILogViewerService, LogViewerService>();
        services.AddSingleton<IFileSystemService, DefaultFileSystemService>();

        // ------------------------------------------------------------------------------
        // Serviços de persistência e acesso a dados
        // ------------------------------------------------------------------------------
        services.AddSingleton<IDatabaseConnectionService, ConnectionTestService>();
        services.AddSingleton<ICsvWriterService, CsvWriterService>();
        services.AddSingleton<IDatabaseSetupService, DatabaseSetupService>();
        services.AddSingleton<IDataRepositoryService, DataRepositoryService>();
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddSingleton<ISchemaManagementService, SqlServerSchemaManagementService>();
        services.AddSingleton<IDataRetrievalService, SqlDataRetrievalService>();
        services.AddSingleton<ITableDiscoveryService, SqlTableDiscoveryService>();
        services.AddSingleton<IZipService, ZipService>();

        services.AddSingleton<IExportResultRepository>(provider =>
        {
            var fileSystemService = provider.GetRequiredService<IFileSystemService>();
            var storagePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ExportadorApp",
                "DadosExportacao"
            );
            fileSystemService.CreateDirectory(storagePath);
            return new FileExportResultRepository(fileSystemService, storagePath);
        });

        // ------------------------------------------------------------------------------
        // Serviços de UI / infraestrutura local
        // ------------------------------------------------------------------------------
        services.AddSingleton<IConnectionParametersStore, ConnectionParametersStore>();
        services.AddSingleton<IConnectionStateService, ConnectionStateService>();
        services.AddSingleton<IExportSettingsRepository, ExportSettingsRepository>();
        services.AddSingleton<IFolderBrowserDialogService, FolderBrowserDialogService>();
        services.AddSingleton<IPasswordProvider, PasswordProvider>();
        services.AddSingleton<IViewDefinitionProvider, SqlServerViewDefinitions>();
        services.AddTransient<IExportContextFactory, ExportContextFactory>();
        services.AddTransient<IExportMappingRepository, InMemoryExportMappingRepository>();

        // ------------------------------------------------------------------------------
        // Casos de uso (Use Cases)
        // ------------------------------------------------------------------------------
        // CORREÇÃO AQUI: Registra a interface para que o DI possa resolvê-la
        services.AddTransient<IManageSelectedTablesUseCase, ManageSelectedTables>();
        services.AddTransient<IExportDataUseCase, ExportData>();   
        services.AddTransient<IExportStep,ValidateConnectionStep>();
        services.AddTransient<IExportStep,SetupDatabaseViewsStep>();
        services.AddTransient<IExportStep, GetSelectedTablesStep>();
        services.AddTransient<IExportStep, ProcessTablesStep>();
        services.AddTransient<IExportStep, CreateZipArchiveStep>();
        services.AddTransient<IExportStep, CleanupTemporaryFilesStep>();
        services.AddTransient<SolidExportOrchestrator>();  

        // ------------------------------------------------------------------------------
        // ViewModels utilizados nas Views
        // ------------------------------------------------------------------------------
        services.AddTransient<ConnectionTestViewModel>();
        services.AddTransient<EntitySelectionViewModel>();
        services.AddTransient<DestinationDirectoryViewModel>();
        services.AddTransient<ExportProcessViewModel>();
        services.AddTransient<ResultViewModel>();
        services.AddTransient<MainWindowViewModel>();

        // ------------------------------------------------------------------------------
        // Views (telas da aplicação WPF)
        // ------------------------------------------------------------------------------
        services.AddTransient<Views.ConnectionTestView>();
        services.AddTransient<Views.EntitySelectionView>();
        services.AddTransient<Views.DestinationDirectoryView>();
        services.AddTransient<Views.ExportProcessView>();
        services.AddTransient<Views.ResultView>();

        // ------------------------------------------------------------------------------
        // Janela principal
        // ------------------------------------------------------------------------------
        services.AddSingleton<MainWindow>();
    }


    /// <summary>
    /// Evento chamado quando a aplicação é encerrada.
    /// Finaliza o logger Serilog e libera o container de serviços.
    /// </summary>
    /// <param name="e">Argumentos do evento de encerramento.</param>
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        Log.CloseAndFlush();
        (_serviceProvider as IDisposable)?.Dispose();
    }
}