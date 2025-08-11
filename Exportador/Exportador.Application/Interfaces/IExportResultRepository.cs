using Exportador.Core.Models;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define o contrato para persistência e recuperação das informações do resultado da exportação.
/// </summary>
public interface IExportResultRepository
{
    /// <summary>
    /// Persiste as informações do resultado da exportação.
    /// </summary>
    /// <param name="result">Objeto contendo os dados da exportação.</param>
    /// <returns>Tarefa assíncrona.</returns>
    Task SaveAsync(ExportResultInfo result);

    /// <summary>
    /// Carrega as informações do último resultado de exportação salvo.
    /// </summary>
    /// <returns>Objeto com os dados do resultado, ou nulo se não existir.</returns>
    Task<ExportResultInfo?> LoadLatestAsync();
}
