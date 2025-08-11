using Exportador.UI.ViewModels;
using System.Windows;

namespace Exportador.UI.Views;

/// <summary>
/// Control de usuário para a interface de seleção do diretório de destino da exportação.
/// Fornece UI para o usuário escolher a pasta onde os arquivos exportados serão salvos.
/// </summary>
public partial class DestinationDirectoryView : System.Windows.Controls.UserControl
{
    /// <summary>
    /// DependencyProperty para controlar o índice atual do passo em um fluxo de etapas (wizard).
    /// Pode ser usado para controlar visibilidade ou comportamento condicional baseado no passo atual.
    /// Nota: Avaliar se essa propriedade deveria estar no ViewModel para melhor aderência ao padrão MVVM.
    /// </summary>
    public static readonly DependencyProperty CurrentStepIndexProperty =
        DependencyProperty.Register(
            "CurrentStepIndex",
            typeof(int),
            typeof(DestinationDirectoryView),
            new PropertyMetadata(0));

    /// <summary>
    /// Propriedade CLR que encapsula a DependencyProperty CurrentStepIndex.
    /// </summary>
    public int CurrentStepIndex
    {
        get => (int)GetValue(CurrentStepIndexProperty);
        set => SetValue(CurrentStepIndexProperty, value);
    }

    /// <summary>
    /// Construtor padrão necessário para inicialização e suporte ao designer.
    /// </summary>
    public DestinationDirectoryView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Construtor que recebe um ViewModel específico para esta View,
    /// associando a lógica e dados ao DataContext para suporte a binding.
    /// </summary>
    /// <param name="viewModel">Instância do DestinationDirectoryViewModel para esta View.</param>
    public DestinationDirectoryView(DestinationDirectoryViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
    }
}