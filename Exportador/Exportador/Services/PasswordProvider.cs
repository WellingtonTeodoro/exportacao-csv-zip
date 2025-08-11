using CommunityToolkit.Mvvm.ComponentModel;
using Exportador.UI.Interfaces;
using System.Security;

namespace Exportador.UI.Services;

/// <summary>
/// Implementação concreta de <see cref="IPasswordProvider"/> para gerenciar a SecureString.
/// É um ObservableObject para permitir que ViewModels se vinculem a mudanças na senha.
/// </summary>
public partial class PasswordProvider : ObservableObject, IPasswordProvider
{ 
    [ObservableProperty]
    private SecureString _password = new SecureString(); 
}