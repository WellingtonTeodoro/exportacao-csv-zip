using CommunityToolkit.Mvvm.ComponentModel;

namespace Exportador.UI.ViewModels;

/// <summary>
/// Representa uma única entidade (tabela) que pode ser selecionada na UI.
/// Encapsula o nome da entidade, seu estado de seleção e o caminho para sua imagem/ícone correspondente.
/// </summary>
public partial class SelectableEntity : ObservableObject
{
    /// <summary>
    /// O nome lógico ou amigável da entidade (ex: "Clientes", "Produtos").
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Indica se a entidade está atualmente selecionada.
    /// Notifica a UI e pode ser vinculada diretamente a um checkbox.
    /// </summary>
    [ObservableProperty]
    private bool _isSelected;

    /// <summary>
    /// O caminho relativo da imagem associada a esta entidade.
    /// Ex: "/Images/cliente.png".
    /// </summary>
    public string ImagePath { get; }  

    /// <summary>
    /// Construtor para criar uma nova instância de SelectableEntity.
    /// </summary>
    /// <param name="name">O nome da entidade.</param>
    /// <param name="isSelected">O estado inicial de seleção da entidade.</param>
    /// <param name="imagePath">O caminho relativo da imagem para esta entidade.</param>
    public SelectableEntity(string name, bool isSelected, string imagePath) 
    {
        Name = name;
        _isSelected = isSelected;
        ImagePath = imagePath;  
    }
}