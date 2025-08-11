using System.Security;

namespace Exportador.UI.Interfaces;

/// <summary>
/// Define a interface para um provedor de senha segura.
/// É usada para desacoplar a PasswordBox de uma SecureString diretamente no ViewModel.
/// </summary>
public interface IPasswordProvider
{
    /// <summary>
    /// Obtém ou define a senha segura encapsulada.
    /// É um SecureString para maior segurança.
    /// A propriedade set deve garantir uma cópia da SecureString para evitar referências diretas.
    /// </summary>
    SecureString Password { get; set; }  
}