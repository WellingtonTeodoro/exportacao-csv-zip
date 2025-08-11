using Exportador.UI.ViewModels;
using UserControl = System.Windows.Controls.UserControl;

namespace Exportador.UI.Views;

/// <summary>
/// UserControl responsável pela interface de teste de conexão ao banco de dados.
/// Fornece os controles visuais para o usuário inserir parâmetros de conexão e executar o teste.
/// </summary>
public partial class ConnectionTestView : UserControl
{
    /// <summary>
    /// Construtor padrão, necessário para inicialização do componente e suporte ao designer.
    /// </summary>
    public ConnectionTestView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Construtor que recebe o ViewModel específico para esta View,
    /// vinculando a lógica de dados e comandos para bindings no XAML.
    /// </summary>
    /// <param name="viewModel">Instância do ConnectionTestViewModel usada como DataContext.</param>
    public ConnectionTestView(ConnectionTestViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
    }
}