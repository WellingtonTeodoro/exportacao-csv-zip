using Exportador.UI.Interfaces;
using Exportador.UI.Services;
using Microsoft.Xaml.Behaviors;  
using System.Runtime.InteropServices;  
using System.Security;  
using System.Windows;  
using System.Windows.Controls;  

namespace Exportador.UI.Behaviors;

/// <summary>
/// Behavior para <see cref="PasswordBox"/> que sincroniza o conteúdo do campo de senha
/// com um provedor de senha seguro, tipicamente um ViewModel que implementa <see cref="IPasswordProvider"/>.
/// Permite o binding bidirecional seguro de uma senha (usando <see cref="SecureString"/>)
/// em um <see cref="PasswordBox"/>, contornando a limitação do `PasswordBox.Password` não ser uma DependencyProperty.
/// </summary>
public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
{
    /// <summary>
    /// <see cref="DependencyProperty"/> que permite associar uma instância de <see cref="IPasswordProvider"/>
    /// a este Behavior via binding XAML. Esta é a propriedade que o ViewModel deve expor.
    /// </summary>
    public static readonly DependencyProperty PasswordProviderProperty =
        DependencyProperty.Register(
            nameof(PasswordProvider),  
            typeof(IPasswordProvider),  
            typeof(PasswordBoxBindingBehavior),  
            new PropertyMetadata(null, OnPasswordProviderChanged));  

    /// <summary>
    /// Obtém ou define a instância do <see cref="IPasswordProvider"/> que este Behavior usará
    /// para ler e escrever a senha de forma segura.
    /// </summary>
    public IPasswordProvider? PasswordProvider
    {
        get => (IPasswordProvider?)GetValue(PasswordProviderProperty);
        set => SetValue(PasswordProviderProperty, value);
    }

    /// <summary>
    /// Chamado quando o Behavior é anexado a um <see cref="PasswordBox"/>.
    /// Este método configura os listeners de eventos e a sincronização inicial.
    /// </summary>
    /// <inheritdoc />
    protected override void OnAttached()
    {
        base.OnAttached(); 
        AssociatedObject.PasswordChanged += OnPasswordChanged; 
        UpdatePasswordBoxFromProvider();
    }

    /// <summary>
    /// Chamado quando o Behavior é desanexado de um <see cref="PasswordBox"/>.
    /// Este método remove os listeners de eventos para evitar vazamentos de memória.
    /// </summary>
    /// <inheritdoc />
    protected override void OnDetaching()
    { 
        AssociatedObject.PasswordChanged -= OnPasswordChanged;
        base.OnDetaching();
    }

    /// <summary>
    /// Método de callback estático invocado quando a propriedade <see cref="PasswordProvider"/> muda.
    /// Garante que o <see cref="PasswordBox"/> seja atualizado com o novo valor do provedor.
    /// </summary>
    /// <param name="d">O <see cref="DependencyObject"/> (neste caso, uma instância de <see cref="PasswordBoxBindingBehavior"/>)
    /// cuja propriedade foi alterada.</param>
    /// <param name="e">Dados do evento de alteração da propriedade de dependência.</param>
    private static void OnPasswordProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    { 
        if (d is PasswordBoxBindingBehavior behavior)
        { 
            behavior.UpdatePasswordBoxFromProvider();
        }
    }

    /// <summary>
    /// Manipulador de eventos para o evento `PasswordChanged` do <see cref="PasswordBox"/>.
    /// Quando a senha no <see cref="PasswordBox"/> é alterada pelo usuário,
    /// este método atualiza a propriedade <see cref="PasswordProvider.Password"/>
    /// com a nova senha de forma segura.
    /// </summary>
    /// <param name="sender">A fonte do evento (o <see cref="PasswordBox"/> associado).</param>
    /// <param name="e">Dados do evento.</param>
    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    { 
        if (PasswordProvider != null)
        { 
            PasswordProvider.Password = AssociatedObject.SecurePassword.Copy();
            System.Diagnostics.Debug.WriteLine("Senha atualizada via Behavior.");
        }
        else
        { 
            System.Diagnostics.Debug.WriteLine("Erro: PasswordProvider no Behavior é nulo ou não definido. Verifique o binding no XAML.");
        }
    }

    /// <summary>
    /// Atualiza o conteúdo do <see cref="PasswordBox"/> com a senha atualmente armazenada
    /// no <see cref="IPasswordProvider"/>. Este método é responsável pela sincronização
    /// do ViewModel para a View (one-way ou inicial).
    /// </summary>
    private void UpdatePasswordBoxFromProvider()
    { 
        if (PasswordProvider != null && AssociatedObject != null)
        { 
            string currentBoxPassword = AssociatedObject.Password; 
            string providerPassword = ConvertSecureStringToString(PasswordProvider.Password);
             
            if (currentBoxPassword != providerPassword)
            { 
                AssociatedObject.Password = providerPassword;
            }
             
            ClearString(providerPassword);
        }
    }

    /// <summary>
    /// Converte um objeto <see cref="SecureString"/> para uma <see cref="string"/> comum.
    /// **ATENÇÃO:** Este método lida com dados sensíveis e deve ser usado com extrema cautela,
    /// minimizando o tempo de existência da string resultante na memória.
    /// </summary>
    /// <param name="secureString">O <see cref="SecureString"/> a ser convertido.</param>
    /// <returns>A representação da senha como <see cref="string"/>, ou <see cref="string.Empty"/> se nulo.</returns>
    private static string ConvertSecureStringToString(SecureString secureString)
    {
        if (secureString == null) return string.Empty;
         
        IntPtr bstr = IntPtr.Zero;
        try
        { 
            bstr = Marshal.SecureStringToBSTR(secureString); 
            return Marshal.PtrToStringBSTR(bstr) ?? string.Empty;
        }
        finally
        { 
            if (bstr != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr);
        }
    }

    /// <summary>
    /// Limpa o conteúdo de uma <see cref="string"/> gerenciada na memória,
    /// substituindo seus caracteres por nulos ('\0'). 
    /// </summary>
    /// <param name="s">A string a ser limpa.</param>
    private static void ClearString(string s)
    {
        if (s == null) return; 
        unsafe
        { 
            fixed (char* charPtr = s)
            { 
                for (int i = 0; i < s.Length; i++)
                {
                    charPtr[i] = '\0';
                }
            }
        }
    }
} 