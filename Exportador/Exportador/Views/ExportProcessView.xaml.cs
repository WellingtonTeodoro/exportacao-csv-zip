using Exportador.UI.ViewModels;

namespace Exportador.UI.Views;

/// <summary>
/// Control de usuário responsável pela interface do processo de exportação.
/// Fornece a interação visual para acompanhar o progresso e status da exportação.
/// </summary>
public partial class ExportProcessView : System.Windows.Controls.UserControl
{
    /// <summary>
    /// Construtor padrão necessário para a inicialização padrão e compatibilidade com o designer.
    /// </summary>
    public ExportProcessView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Construtor que aceita o ViewModel para associar a lógica de apresentação e dados ao controle.
    /// Define o DataContext para permitir binding direto entre View e ViewModel.
    /// </summary>
    /// <param name="viewModel">Instância do ExportProcessViewModel a ser usada como contexto de dados.</param>
    public ExportProcessView(ExportProcessViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
    }
}
