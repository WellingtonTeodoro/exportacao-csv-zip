using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;

namespace Exportador.Application.UseCases;

/// <summary>
/// Objeto de contexto que carrega dados e estado compartilhado durante o processo de exportação.
/// </summary>
public class ExportContext
{
    /// <summary>
    /// Parâmetros de conexão do banco para a exportação atual.
    /// </summary>
    public ConnectionParameters? ConnectionParameters { get; set; }

    /// <summary>
    /// Interface para reportar progresso e status.
    /// </summary>
    public IProgressReporter ProgressReporter { get; }

    /// <summary>
    /// Lista de nomes amigáveis das tabelas selecionadas.
    /// </summary>
    public List<string> SelectedFriendlyTableNames { get; set; } = new();

    /// <summary>
    /// Caminhos completos para arquivos CSV temporários gerados.
    /// </summary>
    public List<string> TemporaryCsvFilePaths { get; } = new();

    /// <summary>
    /// Caminho completo para o arquivo ZIP final gerado.
    /// </summary>
    public string? FinalZipFilePath { get; set; }

    /// <summary>
    /// Armazena a quantidade de registros retornados pela consulta SQL para cada tabela.
    /// </summary>
    public Dictionary<string, int> SqlRecordsCountPerTable { get; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ExportContext"/>.
    /// </summary>
    /// <param name="progressReporter">Instância para reportar progresso.</param>
    public ExportContext(IProgressReporter progressReporter)
    {
        ProgressReporter = progressReporter ?? throw new ArgumentNullException(nameof(progressReporter));
    }
}
