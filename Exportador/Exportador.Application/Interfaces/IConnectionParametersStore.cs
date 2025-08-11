using Exportador.Core.ValueObjects;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Contrato para um serviço que armazena e fornece os parâmetros de conexão do banco de dados.
/// Utilizado para centralizar o acesso aos dados de conexão entre as camadas da aplicação.
/// </summary>
public interface IConnectionParametersStore
{
    /// <summary>
    /// Parâmetros de conexão atuais (ex: servidor, banco, usuário, senha).
    /// Deve ser configurado após um teste de conexão bem-sucedido.
    /// </summary>
    ConnectionParameters CurrentParameters { get; set; }

    /// <summary>
    /// Indica se os parâmetros de conexão já foram definidos (ou seja, CurrentParameters não é nulo).
    /// </summary>
    bool HasParameters { get; }
} 