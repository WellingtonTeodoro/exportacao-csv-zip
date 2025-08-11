using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;

namespace Exportador.Application.UseCases.Steps;

/// <summary>
/// Representa o passo de obtenção da lista de entidades (tabelas) selecionadas para exportação.
/// </summary>
public class GetSelectedTablesStep : IExportStep
{
    /// <summary>
    /// O serviço que fornece os nomes das entidades selecionadas.
    /// </summary>
    private readonly IDataRepositoryService _dataRepo;

    /// <summary>
    /// O serviço de log para registrar informações.
    /// </summary>
    private readonly ILogService _log;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="GetSelectedTablesStep"/>.
    /// </summary>
    /// <param name="dataRepo">A instância do serviço de repositório de dados.</param>
    /// <param name="log">A instância do serviço de log.</param>
    public GetSelectedTablesStep(IDataRepositoryService dataRepo, ILogService log)
    {
        _dataRepo = dataRepo ?? throw new ArgumentNullException(nameof(dataRepo));
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    /// <summary>
    /// Executa a obtenção das entidades selecionadas e as armazena no contexto.
    /// </summary>
    /// <param name="context">O contexto da exportação, que será populado com a lista de nomes de tabelas.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha se nenhuma tabela for selecionada.</returns>
    public Task<Result> ExecuteAsync(ExportContext context)
    {
        context.ProgressReporter.Report(new ExportStatus(10, "Obtendo tabelas selecionadas..."));

        var selectedNames = _dataRepo.GetSelectedEntityNames().ToList();

        if (!selectedNames.Any())
        {
            _log.Warning("Nenhuma tabela foi selecionada para exportação. Processo abortado.");
            return Task.FromResult(Result.Failure("Nenhuma tabela selecionada para exportação."));
        }

        context.SelectedFriendlyTableNames = selectedNames;
        _log.Info($"{selectedNames.Count} tabela(s) selecionada(s) para exportação.");

        return Task.FromResult(Result.Success());
    }
}