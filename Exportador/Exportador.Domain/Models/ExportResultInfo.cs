namespace Exportador.Core.Models;

/// <summary>
/// Representa as informações da exportação de uma tabela específica,
/// incluindo o nome da tabela e a quantidade de registros retornados via SQL.
/// </summary>
public class ExportedTableInfo
{
    /// <summary>
    /// Obtém ou define o nome amigável da tabela exportada.
    /// </summary>
    public string TableName { get; set; } = null!;

    /// <summary>
    /// Obtém ou define a quantidade de registros retornados da consulta SQL para a tabela.
    /// </summary>
    public int SqlRecordsCount { get; set; }
}

/// <summary>
/// Representa o resultado completo da exportação,
/// contendo informações das tabelas exportadas, arquivo ZIP gerado, e tempo total.
/// </summary>
public class ExportResultInfo
{
    /// <summary>
    /// Obtém ou define a lista de informações das tabelas exportadas.
    /// </summary>
    public List<ExportedTableInfo> ExportedTables { get; set; } = new();

    /// <summary>
    /// Obtém a soma total dos registros exportados considerando todas as tabelas.
    /// </summary>
    public int TotalRecords => ExportedTables.Sum(t => t.SqlRecordsCount);

    /// <summary>
    /// Obtém ou define o nome do arquivo ZIP gerado ao final da exportação.
    /// </summary>
    public string ZipFileName { get; set; } = null!;

    /// <summary>
    /// Obtém ou define o tamanho do arquivo ZIP em bytes.
    /// </summary>
    public long ZipFileSizeBytes { get; set; }

    /// <summary>
    /// Obtém ou define o tempo total gasto para executar a exportação.
    /// </summary>
    public TimeSpan TotalExportTime { get; set; }
}
