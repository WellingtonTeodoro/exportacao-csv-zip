using CSharpFunctionalExtensions;
using Exportador.Core.ValueObjects;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define um contrato para serviços que orquestram a configuração inicial
/// de elementos do esquema do banco de dados, como a criação ou atualização de views.
/// Isso desacopla o caso de uso principal (ExportData) dos detalhes de como as views são geridas.
/// </summary>
public interface IDatabaseSetupService
{
    /// <summary>
    /// Configura (cria ou atualiza) todas as views obrigatórias necessárias para
    /// a operação da aplicação, utilizando os parâmetros de conexão fornecidos.
    /// </summary>
    /// <param name="connectionParameters">Os parâmetros de conexão com o banco de dados.</param>
    /// <returns>
    /// Um <see cref="Result"/> indicando sucesso ou falha na operação de setup das views.
    /// </returns>
    Task<Result> SetupRequiredViewsAsync(ConnectionParameters connectionParameters);
}