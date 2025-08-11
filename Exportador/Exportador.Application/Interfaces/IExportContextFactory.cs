using Exportador.Application.UseCases;
using Exportador.Application.UseCases.Steps;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Fábrica responsável por criar o <see cref="ExportContext"/> e os passos (<see cref="IExportStep"/>) 
/// necessários para o processo de exportação de dados.
/// Essa interface permite a separação entre a orquestração e a composição real dos componentes de exportação,
/// aderindo ao princípio da Inversão de Dependência (DIP).
/// </summary>
public interface IExportContextFactory
{
    /// <summary>
    /// Cria uma nova instância de <see cref="ExportContext"/>, 
    /// que encapsula informações e estado compartilhado durante o processo de exportação.
    /// </summary>
    /// <returns>Instância inicializada de <see cref="ExportContext"/>.</returns>
    ExportContext CreateContext(ExportData exportData);

    /// <summary>
    /// Cria e retorna a sequência de passos (<see cref="IExportStep"/>) que compõem o pipeline de exportação.
    /// </summary>
    /// <returns>Sequência ordenada de passos de exportação.</returns>
    IEnumerable<IExportStep> CreateSteps();

    /// <summary>
    /// Cria o passo final responsável pela limpeza de recursos temporários após a exportação,
    /// como arquivos intermediários e diretórios.
    /// </summary>
    /// <returns>Instância de <see cref="IExportStep"/> para limpeza.</returns>
    IExportStep CreateCleanupStep();
}
