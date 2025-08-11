using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;

namespace Exportador.Application.UseCases.Steps;

/// <summary>
/// Representa o passo de validação dos parâmetros de conexão com o banco de dados.
/// Sua responsabilidade é garantir que a aplicação tenha as credenciais necessárias para se conectar
/// antes de iniciar operações mais custosas.
/// </summary>
public class ValidateConnectionStep : IExportStep
{
    /// <summary>
    /// O repositório que armazena os parâmetros de conexão.
    /// </summary>
    private readonly IConnectionParametersStore _connectionStore;

    /// <summary>
    /// O serviço de log para registrar informações da operação.
    /// </summary>
    private readonly ILogService _log;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ValidateConnectionStep"/>.
    /// </summary>
    /// <param name="connectionStore">A instância do repositório de parâmetros de conexão.</param>
    /// <param name="log">A instância do serviço de log.</param>
    public ValidateConnectionStep(IConnectionParametersStore connectionStore, ILogService log)
    {
        _connectionStore = connectionStore ?? throw new ArgumentNullException(nameof(connectionStore));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    /// <summary>
    /// Executa a validação dos parâmetros de conexão.
    /// </summary>
    /// <param name="context">O contexto da exportação, que será populado com os parâmetros de conexão se a validação for bem-sucedida.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha na validação.</returns>
    public Task<Result> ExecuteAsync(ExportContext context)
    {
        context.ProgressReporter.Report(new ExportStatus(0, "Verificando parâmetros de conexão..."));

        if (!_connectionStore.HasParameters)
        {
            const string errorMessage = "Falha ao obter parâmetros de conexão. Por favor, configure e teste a conexão.";
            _log.Error("Nenhum parâmetro de conexão definido. A exportação não pode continuar.");
            return Task.FromResult(Result.Failure(errorMessage));
        }
         
        context.ConnectionParameters = _connectionStore.CurrentParameters;

        _log.Info("Parâmetros de conexão validados com sucesso.");
        context.ProgressReporter.Report(new ExportStatus(5, "Parâmetros de conexão verificados."));
         
        return Task.FromResult(Result.Success());
    }
}