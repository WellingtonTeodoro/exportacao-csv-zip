using CSharpFunctionalExtensions;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define um contrato para um repositório que fornece o mapeamento de exportação.
/// Esta abstração remove os detalhes de configuração da camada de dados (nomes de views, colunas)
/// de dentro da lógica de aplicação (caso de uso), aderindo ao Princípio de Inversão de Dependência (DIP).
/// </summary>
public interface IExportMappingRepository
{
    /// <summary>
    /// Obtém o mapeamento de exportação para uma entidade, dado seu nome amigável.
    /// </summary>
    /// <param name="friendlyName">O nome amigável da tabela/entidade (ex: "Clientes").</param>
    /// <returns>
    /// Um <see cref="Maybe{T}"/> contendo uma tupla com o nome real da tabela/view e a lista de colunas,
    /// ou <see cref="Maybe.None"/> se o mapeamento não for encontrado.
    /// </returns>
    Maybe<(string RealTableName, string SelectColumns)> GetMapping(string friendlyName);
}