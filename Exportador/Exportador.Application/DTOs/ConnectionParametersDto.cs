using System.Security;

namespace Exportador.Application.DTOs;

/// <summary>
/// DTO (Data Transfer Object) para transferir parâmetros de conexão entre camadas.
/// Não contém lógica de negócio, apenas propriedades para transporte de dados.
/// </summary>
public class ConnectionParametersDto
{
    /// <summary>
    /// Nome ou endereço do servidor SQL.
    /// </summary>
    public string Server { get; set; } = string.Empty;

    /// <summary>
    /// Nome do banco de dados a ser acessado.
    /// </summary>
    public string Database { get; set; } = string.Empty;

    /// <summary>
    /// Nome do usuário utilizado para autenticação no banco de dados.
    /// </summary>
    public string User { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário. Utiliza SecureString para manuseio mais seguro na memória.
    /// </summary>
    public SecureString? Password { get; set; }
}