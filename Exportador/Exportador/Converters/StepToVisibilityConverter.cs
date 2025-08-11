using Exportador.UI.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Exportador.UI.Converters;

/// <summary>
/// Conversor que transforma um índice de etapa (step) em um valor de visibilidade para controles UI.
/// Utilizado para controlar a visibilidade de botões baseando-se na etapa atual do fluxo.
/// </summary>
public class StepToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converte o índice da etapa atual e um parâmetro opcional em um valor de <see cref="Visibility"/>.
    /// </summary>
    /// <param name="value">O índice zero-based da etapa atual (espera-se um <see cref="int"/>).</param>
    /// <param name="targetType">O tipo esperado para a saída (geralmente <see cref="Visibility"/>).</param>
    /// <param name="parameter">
    /// Parâmetro opcional que indica qual botão está sendo convertido:
    /// - "LastStep": para o botão "Finalizar", que só deve aparecer na última etapa.
    /// - Qualquer outro valor ou null é tratado como botão "Voltar".
    /// </param>
    /// <param name="culture">Informação de cultura (não utilizada).</param>
    /// <returns>
    /// Retorna <see cref="Visibility.Visible"/> ou <see cref="Visibility.Collapsed"/> de acordo com as regras:
    /// - Botão "Finalizar": visível apenas na última etapa.
    /// - Botão "Voltar": visível se a etapa atual for maior que zero (não na primeira etapa).
    /// - Valor padrão <see cref="Visibility.Collapsed"/> se a entrada for inválida.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int currentStepIndex)
        {
            string? param = parameter as string;
             
            if (param == "LastStep")
            { 
                return currentStepIndex == (GetTotalStepsStatic() - 1) ? Visibility.Visible : Visibility.Collapsed;
            } 
            else
            {
                return currentStepIndex > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
         
        return Visibility.Collapsed;
    }

    /// <summary>
    /// Método não implementado porque a conversão reversa não faz sentido neste contexto.
    /// </summary>
    /// <param name="value">Valor de entrada (não utilizado).</param>
    /// <param name="targetType">Tipo esperado (não utilizado).</param>
    /// <param name="parameter">Parâmetro opcional (não utilizado).</param>
    /// <param name="culture">Cultura (não utilizada).</param>
    /// <returns>Exceção <see cref="NotImplementedException"/> sempre lançada.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retorna o total fixo de etapas do fluxo.
    /// Deve ser mantido sincronizado com o valor usado na <see cref="MainWindowViewModel"/>.
    /// </summary>
    /// <returns>Número total de etapas (4 no caso atual).</returns>
    private static int GetTotalStepsStatic()
    {
        return 4;
    }
}
