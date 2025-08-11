namespace Exportador.Core.ValueObjects;

/// <summary>
/// Objeto de Valor que representa o resultado de um teste de conexão.
/// É imutável e garante consistência.
/// Implementa IEquatable para igualdade baseada em valor.
/// </summary>
public class ConnectionTestResult : IEquatable<ConnectionTestResult>
{
    /// <summary>
    /// Indica se o teste de conexão foi bem-sucedido.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Mensagem de erro detalhada, caso o teste tenha falhado. Pode ser nula em caso de sucesso.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Construtor privado para forçar a criação via métodos estáticos 'Success' ou 'Fail'.
    /// </summary>
    /// <param name="isSuccess">Define se o teste foi bem-sucedido.</param>
    /// <param name="errorMessage">Mensagem de erro, se houver.</param>
    private ConnectionTestResult(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Cria um resultado de teste de conexão bem-sucedido.
    /// </summary>
    public static ConnectionTestResult Success() => new(true, null);

    /// <summary>
    /// Cria um resultado de teste de conexão com falha.
    /// </summary>
    /// <param name="errorMessage">A mensagem de erro detalhada.</param>
    /// <returns>Uma instância de ConnectionTestResult com IsSuccess falso e a mensagem de erro.</returns>
    /// <exception cref="ArgumentException">Lançada se a mensagem de erro for nula ou vazia em caso de falha.</exception>
    public static ConnectionTestResult Fail(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("A mensagem de erro não pode ser nula ou vazia para um resultado de falha.", nameof(errorMessage));

        return new ConnectionTestResult(false, errorMessage);
    }

    /// <summary>
    /// Obtém os componentes usados para comparação de igualdade por valor.
    /// </summary>
    /// <returns>Sequência dos componentes internos.</returns>
    private IEnumerable<object?> GetEqualityComponents()
    {
        yield return IsSuccess;
        yield return ErrorMessage;
    }

    /// <summary>
    /// Compara dois objetos ConnectionTestResult para igualdade por valor.
    /// </summary>
    /// <param name="other">Outro objeto ConnectionTestResult.</param>
    /// <returns>True se forem iguais por valor, caso contrário false.</returns>
    public bool Equals(ConnectionTestResult? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as ConnectionTestResult);

    /// <summary>
    /// Gera um código de hash baseado nos componentes de igualdade.
    /// </summary>
    /// <returns>Hash code do objeto.</returns>
    public override int GetHashCode()
    {
        const int Seed = 19;
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate(Seed, (hash, next) => hash ^ next);
    }

    /// <summary>
    /// Compara dois objetos para igualdade.
    /// </summary>
    public static bool operator ==(ConnectionTestResult? left, ConnectionTestResult? right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    /// <summary>
    /// Compara dois objetos para desigualdade.
    /// </summary>
    public static bool operator !=(ConnectionTestResult? left, ConnectionTestResult? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Permite a conversão implícita de uma <see cref="string"/> para um <see cref="ConnectionTestResult"/>.
    /// </summary>
    /// <remarks>
    /// Atualmente, esta conversão não está implementada e lançará uma <see cref="NotImplementedException"/>.
    /// Em uma implementação futura, ela poderia ser usada para parsear uma string de resultado
    /// de teste de conexão (por exemplo, "Sucesso", "Falha: Mensagem de Erro") em um objeto <see cref="ConnectionTestResult"/>
    /// correspondente.
    /// </remarks>
    /// <param name="v">A string a ser convertida.</param>
    /// <returns>Uma nova instância de <see cref="ConnectionTestResult"/> baseada na string de entrada.</returns>
    /// <exception cref="NotImplementedException">Sempre lançada, pois esta funcionalidade ainda não foi implementada.</exception>
    public static implicit operator ConnectionTestResult(string v)
    {
        throw new NotImplementedException();
    }
}
