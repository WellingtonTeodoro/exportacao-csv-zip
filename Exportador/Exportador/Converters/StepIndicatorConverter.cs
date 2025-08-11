using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Exportador.UI.Converters;

/// <summary>
/// Conversor multi-valor para indicar o estado visual de um passo em um fluxo (step indicator).
/// Baseia-se no índice do passo atual e no índice do passo alvo para definir cores de fundo e texto.
/// Utilizado para alterar a aparência de indicadores visuais, como círculos numerados.
/// </summary>
public class StepIndicatorConverter : IMultiValueConverter
{
    /// <summary>
    /// Converte os valores de entrada (passo atual e passo alvo) em um <see cref="Brushes"/> apropriado
    /// para o fundo ou o texto, dependendo do parâmetro informado.
    /// </summary>
    /// <param name="values">
    /// Array de valores de entrada, espera:
    /// values[0]: índice zero-based do passo atual (int)
    /// values[1]: índice do passo alvo em formato string (1-based)
    /// </param>
    /// <param name="targetType">Tipo do valor de saída esperado (não usado).</param>
    /// <param name="parameter">
    /// Parâmetro do conversor, deve ser "Background" para cor de fundo
    /// ou "Foreground" para cor do texto dentro do indicador.
    /// </param>
    /// <param name="culture">Cultura para conversão (não usada).</param>
    /// <returns>
    /// Retorna um <see cref="Brushes"/> para a cor desejada conforme o estado do passo.
    /// Retorna <see cref="DependencyProperty.UnsetValue"/> em caso de erro ou parâmetro inválido.
    /// </returns>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    { 
        if (values.Length < 2 || !(values[0] is int currentStepIndex) || !(values[1] is string targetStepString))
        { 
            return DependencyProperty.UnsetValue;
        }

        if (!int.TryParse(targetStepString, out int targetStepOneBased))
        {
            return DependencyProperty.UnsetValue;
        }
         
        int targetStepZeroBased = targetStepOneBased - 1;
         
        string? conversionType = parameter as string;
         
        if (conversionType == "Background")
        {
            if (currentStepIndex == targetStepZeroBased)
            { 
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));
            }
            else if (currentStepIndex > targetStepZeroBased)
            { 
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
            }
            else
            { 
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));
            }
        } 
        else if (conversionType == "Foreground")
        {
            if (currentStepIndex == targetStepZeroBased)
            { 
                return Brushes.White;
            }
            else if (currentStepIndex > targetStepZeroBased)
            { 
                return Brushes.White;
            }
            else
            { 
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));
            }
        }
         
        return DependencyProperty.UnsetValue;
    }

    /// <summary>
    /// Método não implementado, pois não há necessidade de conversão reversa.
    /// </summary>
    /// <param name="value">Valor a ser convertido de volta (não usado).</param>
    /// <param name="targetTypes">Tipos esperados para saída (não usado).</param>
    /// <param name="parameter">Parâmetro de conversão (não usado).</param>
    /// <param name="culture">Cultura da conversão (não usado).</param>
    /// <returns>Não implementado, lança <see cref="NotImplementedException"/>.</returns>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
