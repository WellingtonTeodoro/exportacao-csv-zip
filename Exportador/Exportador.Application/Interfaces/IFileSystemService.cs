namespace Exportador.Application.Interfaces;

/// <summary>
/// Abstrai as interações com o sistema de arquivos.
/// Isso remove as dependências diretas de `System.IO` da lógica de aplicação,
/// permitindo maior testabilidade e aderência ao Princípio de Inversão de Dependência (DIP).
/// </summary>
public interface IFileSystemService
{
    /// <summary>
    /// Obtém o caminho para o diretório temporário do sistema.
    /// </summary>
    /// <returns>Uma string com o caminho do diretório temporário.</returns>
    string GetTempPath();

    /// <summary>
    /// Combina um ou mais caminhos em um único caminho de arquivo ou diretório.
    /// </summary>
    /// <param name="paths">Os caminhos a serem combinados.</param>
    /// <returns>O caminho combinado.</returns>
    string Combine(params string[] paths);

    /// <summary>
    /// Cria todos os diretórios e subdiretórios no caminho especificado, a menos que já existam.
    /// </summary>
    /// <param name="path">O caminho do diretório a ser criado.</param>
    void CreateDirectory(string path);

    /// <summary>
    /// Exclui o arquivo especificado.
    /// </summary>
    /// <param name="filePath">O caminho completo do arquivo a ser excluído.</param>
    void DeleteFile(string filePath);

    /// <summary>
    /// Determina se o diretório especificado existe.
    /// </summary>
    /// <param name="path">O caminho do diretório a ser verificado.</param>
    /// <returns><c>true</c> se o diretório existir; caso contrário, <c>false</c>.</returns>
    bool DirectoryExists(string path);

    /// <summary>
    /// Grava assincronamente o conteúdo especificado em um arquivo no caminho fornecido.
    /// </summary>
    /// <remarks>
    /// Se o arquivo no <paramref name="filePath"/> não existir, ele será criado.
    /// Se já existir, seu conteúdo atual será sobrescrito pelo novo <paramref name="content"/>.
    /// </remarks>
    /// <param name="filePath">O caminho completo para o arquivo onde o conteúdo será gravado.</param>
    /// <param name="content">O conteúdo da string a ser gravado no arquivo.</param>
    /// <returns>Uma <see cref="Task"/> que representa a operação de gravação assíncrona.</returns>
    Task WriteAllTextAsync(string filePath, string content);

    /// <summary>
    /// Lê assincronamente todo o texto de um arquivo no caminho especificado.
    /// </summary>
    /// <param name="filePath">O caminho completo para o arquivo de onde o conteúdo será lido.</param>
    /// <returns>Uma <see cref="Task{TResult}"/> que, ao ser concluída, conterá todo o conteúdo do arquivo como uma string.</returns>
    /// <exception cref="System.IO.FileNotFoundException">Lançada se o arquivo especificado por <paramref name="filePath"/> não existir.</exception>
    /// <exception cref="System.IO.IOException">Lançada se ocorrer um erro de I/O durante a operação de leitura.</exception>
    Task<string> ReadAllTextAsync(string filePath);

    /// <summary>
    /// Determina se o arquivo especificado existe.
    /// </summary>
    /// <param name="filePath">O caminho completo para o arquivo a ser verificado.</param>
    /// <returns><c>true</c> se o arquivo existir; caso contrário, <c>false</c>.</returns>
    bool FileExists(string filePath);
}