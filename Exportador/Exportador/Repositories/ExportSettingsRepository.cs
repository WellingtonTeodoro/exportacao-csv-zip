using Exportador.Application.Interfaces;
using Exportador.UI.Properties;
using System.IO;

namespace Exportador.UI.Repositories;

/// <summary>
/// Implementação concreta de <see cref="IExportSettingsRepository"/> utilizando as configurações de aplicação com escopo de usuário.
/// Persiste e recupera configurações relacionadas à exportação, como diretório destino e nome do arquivo.
/// Utiliza as User-Scoped Application Settings do .NET (Settings.Default).
/// </summary>
public class ExportSettingsRepository : IExportSettingsRepository
{
    private const string DefaultFileName = "novo_arquivo_export";
    private const string DefaultFolderName = "Exportador";
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância do repositório de configurações de exportação.
    /// Define valores padrão para diretório e nome do arquivo, caso não estejam configurados.
    /// </summary>
    public ExportSettingsRepository(ILogService logService)
    { 
        if (string.IsNullOrEmpty(Settings.Default.DestinationDirectoryPath))
        {
            Settings.Default.DestinationDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DefaultFolderName);
            Settings.Default.Save();
        } 
        if (string.IsNullOrEmpty(Settings.Default.OutputFileName))
        {
            Settings.Default.OutputFileName = DefaultFileName;
            Settings.Default.Save();
        }

        _logService = logService;
    }

    /// <summary>
    /// Obtém o caminho do diretório onde os arquivos exportados devem ser salvos.
    /// Se o diretório não existir, tenta criá-lo.
    /// Caso falhe, tenta usar o diretório Meus Documentos como fallback.
    /// </summary>
    /// <returns>O caminho absoluto do diretório destino.</returns>
    public string GetDestinationDirectoryPath()
    {
        string path = Settings.Default.DestinationDirectoryPath;
        if (!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            { 
                _logService.Error($"Erro ao criar diretório padrão '{path}': {ex.Message}");
                 
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                 
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                 
                Settings.Default.DestinationDirectoryPath = path;
                Settings.Default.Save();
            }
        }
        return path;
    }

    /// <summary>
    /// Atualiza e persiste o caminho do diretório destino para exportação.
    /// </summary>
    /// <param name="path">Caminho absoluto do diretório.</param>
    public void SetDestinationDirectoryPath(string path)
    {
        Settings.Default.DestinationDirectoryPath = path;
        Settings.Default.Save();
    }

    /// <summary>
    /// Obtém o nome do arquivo (sem extensão) usado para o arquivo compactado gerado na exportação.
    /// </summary>
    /// <returns>Nome do arquivo de saída.</returns>
    public string GetOutputFileName()
    {
        return Settings.Default.OutputFileName;
    }

    /// <summary>
    /// Atualiza e persiste o nome do arquivo de saída da exportação.
    /// </summary>
    /// <param name="fileName">Novo nome do arquivo (sem extensão).</param>
    public void SetOutputFileName(string fileName)
    {
        Settings.Default.OutputFileName = fileName;
        Settings.Default.Save();
    }
}
