using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;
using Exportador.Application.UseCases.Steps;

namespace Exportador.Application.UseCases;

/// <summary>
/// Orquestra a execução de uma sequência de passos de exportação (<see cref="IExportStep"/>).
/// Esta classe adere ao Princípio Aberto/Fechado (OCP), permitindo a extensão do fluxo de exportação
/// por meio da adição de novos passos sem a necessidade de modificar a lógica interna da orquestração.
/// </summary>
public class SolidExportOrchestrator
{
    private IEnumerable<IExportStep> _steps;
    private readonly ILogService _logService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="SolidExportOrchestrator"/>.
    /// </summary>
    /// <param name="steps">Coleção de passos de exportação a serem executados.</param>
    /// <param name="logService">Serviço de logging para rastreamento e diagnóstico.</param>
    public SolidExportOrchestrator(IEnumerable<IExportStep> steps, ILogService logService)
    {
        _steps = steps;
        _logService = logService;
    }

    /// <summary>
    /// Substitui os passos de exportação atuais por uma nova sequência.
    /// Pode ser utilizado para compor diferentes pipelines em tempo de execução.
    /// </summary>
    /// <param name="steps">Nova coleção de passos de exportação.</param>
    public void SetSteps(IEnumerable<IExportStep> steps)
    {
        _steps = steps;
    }

    /// <summary>
    /// Executa sequencialmente todos os passos de exportação definidos.
    /// Caso qualquer passo retorne falha, a execução é interrompida imediatamente.
    /// </summary>
    /// <param name="context">Contexto compartilhado da exportação, contendo configurações, estado e metadados.</param>
    /// <returns>
    /// Um <see cref="Result"/> representando sucesso ou falha na execução do pipeline.
    /// </returns>
    public async Task<Result> ExecuteAsync(ExportContext context)
    {
        foreach (var step in _steps)
        {
            var result = await step.ExecuteAsync(context);
            if (result.IsFailure)
                return result;
        }

        return Result.Success();
    }
}
