namespace Exportador.Application.Interfaces;

/// <summary>
/// Define o contrato para operações de persistência de configurações de exportação.
/// </summary>
public interface IExportSettingsRepository
{
    /// <summary>
    /// Obtém o caminho do diretório de destino salvo nas configurações.
    /// </summary>
    /// <returns>O caminho do diretório de destino.</returns>
    string GetDestinationDirectoryPath();

    /// <summary>
    /// Define e salva o caminho do diretório de destino nas configurações.
    /// </summary>
    /// <param name="path">O novo caminho do diretório.</param>
    void SetDestinationDirectoryPath(string path);

    /// <summary>
    /// Obtém o nome do arquivo de saída salvo nas configurações.
    /// </summary>
    /// <returns>O nome do arquivo de saída.</returns>
    string GetOutputFileName();

    /// <summary>
    /// Define e salva o nome do arquivo de saída nas configurações.
    /// </summary>
    /// <param name="fileName">O novo nome do arquivo.</param>
    void SetOutputFileName(string fileName);
}