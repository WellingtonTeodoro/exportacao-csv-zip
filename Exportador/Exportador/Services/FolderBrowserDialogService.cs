using Exportador.Application.Interfaces;
using Exportador.UI.Interfaces;
using System.Diagnostics;
using System.IO;

namespace Exportador.UI.Services;

/// <summary>
/// Serviço concreto para abrir diálogos de seleção de pasta e abrir pastas no Windows Explorer.
/// Implementa <see cref="IFolderBrowserDialogService"/> usando a API System.Windows.Forms.FolderBrowserDialog e Process.
/// </summary>
public class FolderBrowserDialogService : IFolderBrowserDialogService
{
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância do serviço de diálogo de seleção de pasta.
    /// </summary>
    /// <param name="logService">Serviço de log para registrar eventos e erros.</param>
    /// <exception cref="ArgumentNullException">Se <paramref name="logService"/> for nulo.</exception>
    public FolderBrowserDialogService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    /// <summary>
    /// Abre um diálogo modal para o usuário selecionar uma pasta.
    /// </summary>
    /// <param name="initialPath">
    /// Caminho inicial onde o diálogo deve abrir. Se nulo ou inválido, será usado o diretório Meus Documentos.
    /// </param>
    /// <returns>
    /// Caminho absoluto da pasta selecionada. Retorna nulo se o usuário cancelar ou ocorrer erro.
    /// </returns>
    public string? SelectFolder(string? initialPath = null)
    {
        try
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(initialPath) && Directory.Exists(initialPath))
                {
                    dialog.SelectedPath = initialPath;
                }
                else
                {
                    dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _logService.Debug($"Pasta selecionada via diálogo: {dialog.SelectedPath}");
                    return dialog.SelectedPath;
                }

                _logService.Debug("Seleção de pasta cancelada pelo usuário.");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logService.Error($"Erro ao abrir o diálogo de seleção de pasta: {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Abre o caminho da pasta especificada no Windows Explorer.
    /// </summary>
    /// <param name="folderPath">Caminho absoluto da pasta a ser aberta.</param>
    public void OpenFolderInExplorer(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            _logService.Warning("Tentativa de abrir pasta com caminho nulo ou vazio no Explorer.");
            return;
        }

        try
        {
            string fullPath = Path.GetFullPath(folderPath);

            if (!Directory.Exists(fullPath))
            {
                _logService.Warning($"Tentativa de abrir pasta que não existe no Explorer: '{fullPath}'.");
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = fullPath,
                UseShellExecute = true
            };

            Process.Start(psi);
            _logService.Debug($"Pasta aberta no Explorer: '{fullPath}'.");
        }
        catch (System.ComponentModel.Win32Exception ex)
        {
            _logService.Error($"Erro Win32 ao abrir a pasta '{folderPath}' no Explorer: {ex.Message}. Verifique se 'explorer.exe' está acessível.", ex);
        }
        catch (Exception ex)
        {
            _logService.Error($"Erro inesperado ao tentar abrir a pasta '{folderPath}' no Explorer: {ex.Message}", ex);
        }
    }
} 