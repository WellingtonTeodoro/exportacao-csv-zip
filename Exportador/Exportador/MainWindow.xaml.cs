using Exportador.UI.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Exportador.UI;

/// <summary>
/// Janela principal da aplicação.
/// Responsável por inicializar a interface e configurar o contexto de dados com o ViewModel principal.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Construtor da MainWindow que recebe o ViewModel principal via injeção de dependência.
    /// </summary>
    /// <param name="viewModel">Instância do MainWindowViewModel fornecida pelo container de DI.</param>
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel; 
    }

    /// <summary>
    ///  1. Verifica se o DataContext da janela implementa a interface IDisposable.
    ///   O DataContext é o objeto (geralmente um ViewModel) ao qual os elementos da UI estão vinculados.
    ///  2. Se o DataContext implementa IDisposable, chama seu método Dispose().
    ///   Isso é crucial para liberar recursos e desassinar eventos, prevenindo memory leaks.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        
        if (DataContext is IDisposable disposableViewModel)
        { 
            disposableViewModel.Dispose();
        }
    }
}
