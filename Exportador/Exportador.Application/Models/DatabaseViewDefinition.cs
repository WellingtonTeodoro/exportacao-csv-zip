namespace Exportador.Application.Models;

/// <summary>
/// Representa a definição de uma view de banco de dados, incluindo seu nome e
/// a string SQL que define sua estrutura. É um objeto de valor imutável.
/// </summary>
public class DatabaseViewDefinition
{
    /// <summary>
    /// Obtém o nome completo da view (ex: "dbo.MinhaView").
    /// </summary>
    public string ViewName { get; }

    /// <summary>
    /// Obtém a definição SQL da view (a parte SELECT, sem o "CREATE VIEW ViewName AS").
    /// </summary>
    public string ViewSqlDefinition { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="DatabaseViewDefinition"/>.
    /// </summary>
    /// <param name="viewName">O nome completo da view. Não pode ser nulo ou vazio.</param>
    /// <param name="viewSqlDefinition">A string SQL que define a view. Não pode ser nula ou vazia.</param>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="viewName"/> ou <paramref name="viewSqlDefinition"/> forem nulos.</exception>
    /// <exception cref="ArgumentException">Lançada se <paramref name="viewName"/> ou <paramref name="viewSqlDefinition"/> forem vazios ou apenas espaços em branco.</exception>
    public DatabaseViewDefinition(string viewName, string viewSqlDefinition)
    {
        ViewName = !string.IsNullOrWhiteSpace(viewName)
            ? viewName
            : throw new ArgumentException("O nome da exibição não pode ser nulo ou ter espaços em branco.", nameof(viewName));
        ViewSqlDefinition = !string.IsNullOrWhiteSpace(viewSqlDefinition)
            ? viewSqlDefinition
            : throw new ArgumentException("A definição de SQL de exibição não pode ser nula ou ter espaços em branco.", nameof(viewSqlDefinition));
    }
}