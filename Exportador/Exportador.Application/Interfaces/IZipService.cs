namespace Exportador.Application.Interfaces;

/// <summary>
/// Define um serviço para compactar arquivos em formato ZIP.
/// </summary>
public interface IZipService
{
    /// <summary>
    /// Cria um arquivo ZIP contendo os arquivos especificados.
    /// </summary>
    /// <param name="zipFilePath">O caminho completo onde o arquivo ZIP será criado.</param>
    /// <param name="filesToCompress">Uma lista de caminhos completos dos arquivos a serem adicionados ao ZIP.</param>
    void CreateZipFile(string zipFilePath, IEnumerable<string> filesToCompress);
}