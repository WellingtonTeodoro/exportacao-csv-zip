using Exportador.UI.ViewModels;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace Exportador.UI.Views;

/// <summary>
/// Controle de usuário responsável por exibir os resultados do processo de exportação.
/// </summary>
public partial class ResultView : UserControl
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ResultView"/>.
    /// Este construtor é responsável por configurar os componentes da interface de usuário
    /// e subscrever o evento 'Loaded' para carregar dados e exibir resultados
    /// quando a view estiver completamente carregada na UI.
    /// </summary>
    public ResultView()
    {
        InitializeComponent();
        Loaded += OnLoadedAsync;
    }

    private async void OnLoadedAsync(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoadedAsync;

        if (DataContext is ResultViewModel viewModel)
        {
            try
            {
                await viewModel.LoadAsync();
            }
            catch (Exception ex)
            {
                viewModel._logService?.Error("Erro ao carregar os dados da exportação", ex);
            }
        }
    }
}
