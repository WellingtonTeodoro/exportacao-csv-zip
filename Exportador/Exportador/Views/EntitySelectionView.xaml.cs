using Exportador.UI.ViewModels;

namespace Exportador.UI.Views;

/// <summary>
/// Control de usuário responsável pela interface de seleção de entidades.
/// Fornece a interface para que o usuário escolha as tabelas ou entidades a serem exportadas.
/// </summary>
public partial class EntitySelectionView : System.Windows.Controls.UserControl
{
    /// <summary>
    /// Construtor padrão necessário para inicialização e suporte ao designer.
    /// </summary>
    public EntitySelectionView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Construtor que recebe o ViewModel para associar a lógica de apresentação e dados à View.
    /// Define o DataContext para permitir a ligação de dados (data binding) entre a View e o ViewModel.
    /// </summary>
    /// <param name="viewModel">Instância do EntitySelectionViewModel que contém a lógica e os dados para esta View.</param>
    public EntitySelectionView(EntitySelectionViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
    }
}
