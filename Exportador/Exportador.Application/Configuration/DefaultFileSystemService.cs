using Exportador.Application.Interfaces;
using System.IO;

namespace Exportador.Application.Configuration;

/// <summary>
/// Uma implementação padrão do serviço de sistema de arquivos que utiliza `System.IO`.
/// </summary>
public class DefaultFileSystemService : IFileSystemService
{
    /// <summary>
    /// Obtém o caminho do diretório temporário do usuário atual.
    /// </summary>
    /// <returns>O caminho para o diretório temporário.</returns>
    public string GetTempPath() => Path.GetTempPath();

    /// <summary>
    /// Combina uma matriz de strings de caminho em um único caminho.
    /// </summary>
    /// <remarks>
    /// Este método lida com delimitadores de diretório e caminhos relativos de forma inteligente.
    /// </remarks>
    /// <param name="paths">Uma matriz de strings de caminho a serem combinadas.</param>
    /// <returns>O caminho combinado.</returns>
    public string Combine(params string[] paths) => Path.Combine(paths);

    /// <summary>
    /// Cria todos os diretórios e subdiretórios no caminho especificado.
    /// </summary>
    /// <remarks>
    /// Se o diretório já existir, este método não faz nada.
    /// </remarks>
    /// <param name="path">O caminho do diretório a ser criado.</param>
    public void CreateDirectory(string path) => Directory.CreateDirectory(path);

    /// <summary>
    /// Exclui o arquivo especificado.
    /// </summary>
    /// <remarks>
    /// Se o arquivo não existir, nenhuma exceção é lançada.
    /// </remarks>
    /// <param name="filePath">O caminho completo do arquivo a ser excluído.</param>
    public void DeleteFile(string filePath) => File.Delete(filePath);

    /// <summary>
    /// Determina se o diretório especificado existe.
    /// </summary>
    /// <param name="path">O caminho do diretório a ser verificado.</param>
    /// <returns><c>true</c> se o diretório existir; caso contrário, <c>false</c>.</returns>
    public bool DirectoryExists(string path) => Directory.Exists(path);

    /// <summary>
    /// Grava assincronamente o conteúdo de texto fornecido em um arquivo no caminho especificado.
    /// </summary>
    /// <remarks>
    /// Este método cria o arquivo se ele não existir, ou sobrescreve-o se já existir.
    /// </remarks>
    /// <param name="filePath">O caminho completo para o arquivo onde o conteúdo será gravado.</param>
    /// <param name="content">O conteúdo de texto a ser gravado no arquivo.</param>
    /// <returns>Uma <see cref="Task"/> que representa a operação de gravação assíncrona.</returns>
    public async Task WriteAllTextAsync(string filePath, string content)
            => await File.WriteAllTextAsync(filePath, content);

    /// <summary>
    /// Lê assincronamente todo o conteúdo de texto de um arquivo no caminho especificado.
    /// </summary>
    /// <param name="filePath">O caminho completo para o arquivo de onde o conteúdo será lido.</param>
    /// <returns>Uma <see cref="Task{TResult}"/> que, ao ser concluída, conterá todo o conteúdo do arquivo como uma string.</returns>
    /// <exception cref="System.IO.FileNotFoundException">Lançada se o arquivo especificado em <paramref name="filePath"/> não for encontrado.</exception>
    /// <exception cref="System.IO.IOException">Lançada se ocorrer um erro de I/O durante a operação de leitura.</exception>
    public async Task<string> ReadAllTextAsync(string filePath)
            => await File.ReadAllTextAsync(filePath);

    /// <summary>
    /// Determina se um arquivo existe no caminho especificado.
    /// </summary>
    /// <param name="filePath">O caminho completo para o arquivo a ser verificado.</param>
    /// <returns><c>true</c> se o arquivo existir; caso contrário, <c>false</c>.</returns>
    public bool FileExists(string filePath)
            => File.Exists(filePath);
}