using CSharpFunctionalExtensions;
using System.Security;  

namespace Exportador.Core.ValueObjects;

/// <summary>
/// Objeto de Valor que encapsula os parâmetros de conexão com um banco de dados.
/// É imutável e garante a validade dos parâmetros na sua construção.
/// Implementa <see cref="IEquatable{T}"/> para igualdade baseada em valor.
/// </summary>
public class ConnectionParameters : IEquatable<ConnectionParameters>
{
    /// <summary>
    /// Nome do servidor de banco de dados.
    /// </summary>
    public string Server { get; }

    /// <summary>
    /// Nome do banco de dados.
    /// </summary>
    public string Database { get; }

    /// <summary>
    /// Nome de usuário para autenticação.
    /// </summary>
    public string User { get; }

    /// <summary>
    /// Senha protegida do usuário.
    /// </summary>
    public SecureString Password { get; }

    /// <summary>
    /// Construtor privado para forçar o uso do método <see cref="Create"/>.
    /// </summary>
    /// <param name="server">Nome do servidor.</param>
    /// <param name="database">Nome do banco de dados.</param>
    /// <param name="user">Usuário para autenticação.</param>
    /// <param name="password">Senha do usuário.</param>
    private ConnectionParameters(string server, string database, string user, SecureString password)
    {
        Server = server;
        Database = database;
        User = user;
        Password = password;
    }

    /// <summary>
    /// Cria uma instância válida de <see cref="ConnectionParameters"/>, aplicando validações.
    /// </summary>
    /// <param name="server">Nome do servidor.</param>
    /// <param name="database">Nome do banco de dados.</param>
    /// <param name="user">Nome do usuário.</param>
    /// <param name="password">Senha protegida.</param>
    /// <returns>Resultado contendo a instância criada ou erro de validação.</returns>
    public static Result<ConnectionParameters> Create(string server, string database, string user, SecureString password)
    {
        if (string.IsNullOrWhiteSpace(server))
            return Result.Failure<ConnectionParameters>("O nome do servidor não pode ser vazio.");
        if (string.IsNullOrWhiteSpace(database))
            return Result.Failure<ConnectionParameters>("O nome do banco de dados não pode ser vazio.");
        if (string.IsNullOrWhiteSpace(user))
            return Result.Failure<ConnectionParameters>("O nome de usuário não pode ser vazio."); 
        if (password == null || password.Length == 0)
            return Result.Failure<ConnectionParameters>("A senha não pode ser vazia.");

        return Result.Success(new ConnectionParameters(server, database, user, password));
    }

    /// <summary>
    /// Retorna os componentes que definem igualdade para esse objeto.
    /// </summary>
    /// <returns>Componentes de igualdade.</returns>
    private IEnumerable<object?> GetEqualityComponents()
    {
        yield return Server;
        yield return Database;
        yield return User; 
        yield return Password != null && Password.Length > 0;
    }

    /// <summary>
    /// Compara este objeto com outro <see cref="ConnectionParameters"/> para verificar igualdade de valor.
    /// </summary>
    /// <param name="other">Outro objeto para comparar.</param>
    /// <returns>True se forem iguais em valor; caso contrário, false.</returns>
    public bool Equals(ConnectionParameters? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Compara este objeto com outro qualquer.
    /// </summary>
    /// <param name="obj">Outro objeto.</param>
    /// <returns>True se forem iguais; caso contrário, false.</returns>
    public override bool Equals(object? obj) => Equals(obj as ConnectionParameters);

    /// <summary>
    /// Retorna o hash code calculado com base nos componentes de valor.
    /// </summary>
    /// <returns>Hash code do objeto.</returns>
    public override int GetHashCode()
    {
        const int Seed = 17;
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate(Seed, (hash, next) => hash ^ next);
    }

    /// <summary>
    /// Operador de igualdade entre duas instâncias de <see cref="ConnectionParameters"/>.
    /// </summary>
    /// <param name="left">Primeira instância.</param>
    /// <param name="right">Segunda instância.</param>
    /// <returns>True se forem iguais; caso contrário, false.</returns>
    public static bool operator ==(ConnectionParameters? left, ConnectionParameters? right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    /// <summary>
    /// Operador de diferença entre duas instâncias de <see cref="ConnectionParameters"/>.
    /// </summary>
    /// <param name="left">Primeira instância.</param>
    /// <param name="right">Segunda instância.</param>
    /// <returns>True se forem diferentes; caso contrário, false.</returns>
    public static bool operator !=(ConnectionParameters? left, ConnectionParameters? right)
    {
        return !(left == right);
    } 
}