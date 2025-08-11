using CSharpFunctionalExtensions;

namespace Exportador.Application.UseCases.Steps;

/// <summary>
/// Define o contrato para um passo atômico e executável dentro do fluxo de exportação de dados.
/// A implementação deste contrato permite a criação de um pipeline flexível e extensível,
/// aderindo ao Princípio Aberto/Fechado (OCP).
/// </summary>
public interface IExportStep
{
    /// <summary>
    /// Executa a lógica específica deste passo de forma assíncrona.
    /// </summary>
    /// <param name="context">O objeto de contexto que carrega o estado compartilhado entre todos os passos da exportação.</param>
    /// <returns>Um <see cref="Result"/> que indica o sucesso ou a falha da execução do passo.</returns>
    Task<Result> ExecuteAsync(ExportContext context);
}