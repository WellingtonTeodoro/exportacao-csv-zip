using Exportador.Application.Interfaces;
using Exportador.Core.ValueObjects;

namespace Exportador.Application.UseCases.Steps;

/// <summary>
/// Objeto de contexto que flui através de todos os passos da exportação.
/// Ele carrega o estado compartilhado da operação, como parâmetros de conexão,
/// listas de arquivos e o reporter de progresso.
/// </summary>
public class ExportContextStep
{
    /// <summary>
    /// Obtém ou define os parâmetros de conexão com o banco de dados para a exportação atual.
    /// </summary>
    public ConnectionParameters? ConnectionParameters { get; set; }

    /// <summary>
    /// Obtém a instância do reporter de progresso para notificar sobre o andamento da exportação.
    /// </summary>
    public IProgressReporter ProgressReporter { get; }

    /// <summary>
    /// Obtém ou define a lista de nomes amigáveis das tabelas selecionadas para exportação.
    /// </summary>
    public List<string> SelectedFriendlyTableNames { get; set; } = new();

    /// <summary>
    /// Obtém a lista dos caminhos completos para os arquivos CSV temporários gerados.
    /// </summary>
    public List<string> TemporaryCsvFilePaths { get; } = new();

    /// <summary>
    /// Obtém ou define o caminho completo onde o arquivo ZIP final será salvo.
    /// </summary>
    public string? FinalZipFilePath { get; set; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ExportContext"/>.
    /// </summary>
    /// <param name="progressReporter">A implementação do reporter de progresso a ser usada.</param>
    public ExportContextStep(IProgressReporter progressReporter)
    {
        ProgressReporter = progressReporter ?? throw new ArgumentNullException(nameof(progressReporter));
    }
}