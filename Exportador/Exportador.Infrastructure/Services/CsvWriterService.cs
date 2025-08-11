using Exportador.Application.Interfaces;
using System.IO;
using System.Text;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Serviço responsável por gerar arquivos CSV a partir de uma lista de dicionários contendo os dados.
/// </summary>
public class CsvWriterService : ICsvWriterService
{
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CsvWriterService"/>.
    /// </summary>
    /// <param name="logService">Serviço de logging utilizado para registrar operações e erros.</param>
    public CsvWriterService(ILogService logService)
    {
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("CsvWriterService inicializado.");
    }

    /// <summary>
    /// Gera um arquivo CSV com base nos dados fornecidos.
    /// </summary>
    /// <param name="filePath">Caminho completo onde o arquivo CSV será salvo.</param>
    /// <param name="data">Lista de dicionários representando as linhas do CSV. As chaves são os nomes das colunas.</param>
    /// <returns>Uma <see cref="Task"/> representando a operação assíncrona.</returns>
    /// <exception cref="ArgumentException">Lançada se o caminho for nulo ou vazio.</exception>
    /// <exception cref="InvalidOperationException">Lançada em caso de falha de I/O ou erro inesperado.</exception>
    public async Task WriteCsvAsync(string filePath, List<Dictionary<string, object>> data)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("O caminho do arquivo não pode ser nulo ou vazio.", nameof(filePath));
        if (data == null || !data.Any())
        {
            _logService.Warning($"Nenhum dado fornecido para escrita CSV em '{filePath}'. Criando arquivo vazio.");
            await File.WriteAllTextAsync(filePath, "", Encoding.UTF8);
            return;
        }

        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                _logService.Debug($"Diretório '{directory}' criado para o arquivo CSV.");
            }

            var headers = data.First().Keys.ToList();
            var csvContent = new StringBuilder();
             
            csvContent.AppendLine(string.Join(";", headers.Select(h => EscapeCsvField(h))));

            foreach (var row in data)
            {
                var line = new List<string>();
                foreach (var header in headers)
                {
                    row.TryGetValue(header, out object? value);
                    line.Add(EscapeCsvField(value?.ToString() ?? string.Empty));
                } 
                csvContent.AppendLine(string.Join(";", line));
            }

            await File.WriteAllTextAsync(filePath, csvContent.ToString(), Encoding.UTF8);
            _logService.Info($"Arquivo CSV criado com sucesso: '{filePath}' com {data.Count} linhas.");
        }
        catch (IOException ioEx)
        {
            _logService.Error(ioEx, $"Erro de I/O ao escrever arquivo CSV em '{filePath}'. Verifique permissões ou se o arquivo está em uso. Detalhes: {ioEx.Message}");
            throw new InvalidOperationException($"Erro de I/O ao escrever arquivo CSV: {ioEx.Message}", ioEx);
        }
        catch (UnauthorizedAccessException uaEx)
        {
            _logService.Error(uaEx, $"Acesso negado ao escrever arquivo CSV em '{filePath}'. Verifique as permissões do usuário. Detalhes: {uaEx.Message}");
            throw new InvalidOperationException($"Permissão negada ao escrever arquivo CSV: {uaEx.Message}", uaEx);
        }
        catch (Exception ex)
        {
            _logService.Error(ex, $"Erro inesperado ao escrever arquivo CSV em '{filePath}'. Detalhes: {ex.Message}");
            throw new InvalidOperationException($"Erro inesperado ao escrever arquivo CSV: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Escapa um campo de texto para o formato CSV. Adiciona aspas duplas se o campo contiver ponto e vírgula, aspas ou quebras de linha.
    /// </summary>
    /// <param name="field">Campo a ser tratado.</param>
    /// <returns>Campo formatado para inserção segura no CSV.</returns>
    private string EscapeCsvField(string field)
    {
        if (string.IsNullOrEmpty(field))
            return string.Empty;
         
        if (field.Contains(";") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}