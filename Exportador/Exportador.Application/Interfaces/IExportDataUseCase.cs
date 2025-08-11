using CSharpFunctionalExtensions; 

namespace Exportador.Application.UseCases;

/// <summary>
/// Define o contrato para o caso de uso de exportação de dados.
/// Essa interface representa o ponto de entrada da aplicação para iniciar o processo de exportação completo.
/// </summary>
public interface IExportDataUseCase
{
    /// <summary>
    /// Evento disparado para notificar o progresso da exportação (0-100).
    /// </summary>
    event EventHandler<int> ProgressUpdated;

    /// <summary>
    /// Evento disparado para notificar mensagens de status durante a exportação.
    /// </summary>
    event EventHandler<string> StatusMessageUpdated;

    /// <summary>
    /// Executa o processo de exportação de dados de forma assíncrona,
    /// orquestrando todos os passos necessários, desde a leitura até a geração dos arquivos.
    /// </summary>
    /// <returns>
    /// Um <see cref="Result"/> representando o sucesso ou falha da operação.
    /// Caso falhe, o erro estará descrito na propriedade <c>Error</c> do resultado.
    /// </returns>
    Task<Result> ExecuteExportAsync();
}