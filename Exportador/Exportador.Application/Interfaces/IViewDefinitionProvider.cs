using Exportador.Application.Models;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define um provedor de definições de views que devem existir no banco de dados.
/// </summary>
public interface IViewDefinitionProvider
{
    /// <summary>
    /// Obtém a lista de definições de views que devem ser garantidas ou geradas no banco de dados.
    /// </summary>
    /// <returns>
    /// Uma lista somente leitura contendo objetos <see cref="DatabaseViewDefinition"/>, cada um representando a definição de uma view SQL.
    /// </returns>
    IReadOnlyList<DatabaseViewDefinition> GetRequiredViews();
}
