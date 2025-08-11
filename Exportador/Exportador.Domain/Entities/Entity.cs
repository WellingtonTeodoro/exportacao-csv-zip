namespace Exportador.Core.Entities;

/// <summary>
/// Representa uma entidade base com identidade do tipo <typeparamref name="TId"/>.
/// Entidades são objetos com identidade própria e são comparadas por ID.
/// </summary>
/// <typeparam name="TId">Tipo da chave primária da entidade (ex: int, Guid).</typeparam>
public abstract class Entity<TId> where TId : IEquatable<TId>
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Inicializa uma nova instância da entidade com o identificador fornecido.
    /// </summary>
    /// <param name="id">Valor que será usado como identificador da entidade.</param>
    protected Entity(TId id)
    {
        Id = id;
    } 
}
