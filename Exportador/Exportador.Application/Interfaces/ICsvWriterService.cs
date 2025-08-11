namespace Exportador.Application.Interfaces;

/// <summary>
/// Define um serviço para escrita de dados em formato CSV.
/// </summary>
public interface ICsvWriterService
{
    /// <summary>
    /// Escreve uma lista de dicionários (representando linhas e colunas) em um arquivo CSV.
    /// A primeira linha do CSV será o cabeçalho, com os nomes das chaves dos dicionários.
    /// </summary>
    /// <param name="filePath">O caminho completo onde o arquivo CSV será salvo.</param>
    /// <param name="data">A lista de dicionários, onde cada dicionário é uma linha e as chaves são os nomes das colunas.</param>
    /// <returns>Uma Task que representa a operação assíncrona.</returns>
    Task WriteCsvAsync(string filePath, List<Dictionary<string, object>> data);
}