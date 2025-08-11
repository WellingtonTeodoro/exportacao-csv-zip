using Exportador.Application.Interfaces;
using Exportador.Core.Models;
using System.Text.Json;

namespace Exportador.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="IExportResultRepository"/>  
/// </summary>
public class FileExportResultRepository : IExportResultRepository
{
    private readonly IFileSystemService _fileSystem;
    private readonly string _filePath;

    /// <summary>
    /// Inicializa uma nova instância do repositório de resultado da exportação.
    /// </summary>
    /// <param name="fileSystem">Serviço de manipulação do sistema de arquivos.</param>
    /// <param name="storagePath">Caminho da pasta onde o arquivo será salvo.</param>
    public FileExportResultRepository(IFileSystemService fileSystem, string storagePath)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        if (string.IsNullOrWhiteSpace(storagePath))
            throw new ArgumentException("O caminho de armazenamento não pode ser vazio.", nameof(storagePath));

        _filePath = _fileSystem.Combine(storagePath, "export_result.json");
    }

    /// <summary>
    /// Salva assincronamente as informações de um resultado de exportação em um arquivo.
    /// </summary>
    /// <remarks>
    /// O objeto <paramref name="result"/> é serializado para uma string JSON formatada (com indentação)
    /// e então gravado no arquivo especificado por <c>_filePath</c> usando o sistema de arquivos.
    /// </remarks>
    /// <param name="result">O objeto <see cref="ExportResultInfo"/> contendo os dados a serem salvos.</param>
    /// <returns>Uma <see cref="Task"/> que representa a operação de salvamento assíncrona.</returns>
    public async Task SaveAsync(ExportResultInfo result)
    {
        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        await _fileSystem.WriteAllTextAsync(_filePath, json);
    }

    /// <summary>
    /// Carrega assincronamente as informações do último resultado de exportação de um arquivo.
    /// </summary>
    /// <remarks>
    /// Primeiro, verifica se o arquivo especificado por <c>_filePath</c> existe.
    /// Se o arquivo não existir, retorna <c>null</c>.
    /// Caso contrário, lê todo o conteúdo do arquivo como uma string JSON e o desserializa
    /// para um objeto <see cref="ExportResultInfo"/>.
    /// </remarks>
    /// <returns>
    /// Uma <see cref="Task{TResult}"/> que, ao ser concluída, conterá o objeto <see cref="ExportResultInfo"/>
    /// carregado, ou <c>null</c> se o arquivo não existir.
    /// </returns>
    public async Task<ExportResultInfo?> LoadLatestAsync()
    {
        if (!_fileSystem.FileExists(_filePath))
            return null;

        var json = await _fileSystem.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<ExportResultInfo>(json);
    }
}
