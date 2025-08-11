using Exportador.Application.Interfaces;
using System.IO;
using System.IO.Compression;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Implementação do serviço <see cref="IZipService"/> para compactação de arquivos em formato ZIP.
/// </summary>
public class ZipService : IZipService
{
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ZipService"/>.
    /// </summary>
    /// <param name="logService">Serviço de logging para registrar eventos e erros.</param>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="logService"/> for nulo.</exception>
    public ZipService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("ZipService inicializado.");
    }

    /// <summary>
    /// Cria um arquivo ZIP contendo os arquivos especificados.
    /// </summary>
    /// <param name="zipFilePath">Caminho completo do arquivo ZIP a ser criado.</param>
    /// <param name="filesToCompress">Coleção de caminhos dos arquivos que serão incluídos no ZIP.</param>
    /// <exception cref="ArgumentException">Lançada se o caminho do ZIP for nulo ou vazio.</exception>
    /// <exception cref="ArgumentNullException">Lançada se a lista de arquivos for nula.</exception>
    /// <exception cref="InvalidOperationException">Lançada em caso de falha ao criar o ZIP.</exception>
    public void CreateZipFile(string zipFilePath, IEnumerable<string> filesToCompress)
    {
        if (string.IsNullOrWhiteSpace(zipFilePath))
            throw new ArgumentException("O caminho do arquivo ZIP não pode ser nulo ou vazio.", nameof(zipFilePath));

        if (filesToCompress == null)
            throw new ArgumentNullException(nameof(filesToCompress), "A lista de arquivos a compactar não pode ser nula.");

        try
        { 
            var directory = Path.GetDirectoryName(zipFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                _logService.Debug($"Diretório '{directory}' criado para o arquivo ZIP.");
            }
             
            using (var zipStream = new FileStream(zipFilePath, FileMode.Create))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var filePath in filesToCompress)
                {
                    if (File.Exists(filePath))
                    {
                        var entryName = Path.GetFileName(filePath);  
                        archive.CreateEntryFromFile(filePath, entryName, CompressionLevel.Optimal);
                        _logService.Debug($"Arquivo '{filePath}' adicionado ao ZIP como '{entryName}'.");
                    }
                    else
                    { 
                        _logService.Warning($"Arquivo '{filePath}' não encontrado e não pode ser adicionado ao ZIP. Continuado sem ele.");
                    }
                }
            }

            _logService.Info($"Arquivo ZIP '{zipFilePath}' criado com sucesso com {filesToCompress.Count(File.Exists)} arquivos (considerando apenas os existentes).");
        }
        catch (IOException ioEx)
        { 
            _logService.Error(ioEx, $"Erro de I/O ao criar arquivo ZIP em '{zipFilePath}'. Verifique permissões, espaço em disco ou se o arquivo está em uso por outro processo. Detalhes: {ioEx.Message}");
            throw new InvalidOperationException($"Erro de I/O ao criar arquivo ZIP: {ioEx.Message}", ioEx);
        }
        catch (UnauthorizedAccessException uaEx)
        { 
            _logService.Error(uaEx, $"Acesso negado ao criar ou escrever arquivo ZIP em '{zipFilePath}'. Verifique as permissões do usuário. Detalhes: {uaEx.Message}");
            throw new InvalidOperationException($"Permissão negada ao criar arquivo ZIP: {uaEx.Message}", uaEx);
        }
        catch (Exception ex)
        { 
            _logService.Error(ex, $"Erro inesperado ao criar arquivo ZIP em '{zipFilePath}'. Detalhes: {ex.Message}");
            throw new InvalidOperationException($"Erro inesperado ao criar arquivo ZIP: {ex.Message}", ex);
        }
    }
}