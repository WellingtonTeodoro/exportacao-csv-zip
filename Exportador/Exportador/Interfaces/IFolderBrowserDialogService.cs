namespace Exportador.UI.Interfaces;

/// <summary>
/// Interface para serviço de diálogo que permite selecionar pastas no sistema de arquivos.
/// Define operações para abrir o diálogo de seleção e abrir pastas no explorador de arquivos.
/// </summary>
public interface IFolderBrowserDialogService
{
    /// <summary>
    /// Abre um diálogo para o usuário selecionar uma pasta.
    /// </summary>
    /// <param name="initialPath">
    /// Caminho inicial onde o diálogo deve abrir. Pode ser nulo para usar a pasta padrão do sistema.
    /// </param>
    /// <returns>
    /// Retorna o caminho absoluto da pasta selecionada pelo usuário.
    /// Retorna nulo ou string vazia se o usuário cancelar a operação.
    /// </returns>
    string? SelectFolder(string? initialPath = null);

    /// <summary>
    /// Abre a pasta especificada no Windows Explorer.
    /// </summary>
    /// <param name="folderPath">Caminho absoluto da pasta que será aberta.</param>
    void OpenFolderInExplorer(string folderPath);
}
