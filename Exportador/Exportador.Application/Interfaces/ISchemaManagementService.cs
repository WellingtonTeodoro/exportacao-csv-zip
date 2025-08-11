using CSharpFunctionalExtensions;
using Exportador.Core.ValueObjects;

namespace Exportador.Application.Interfaces;

/// <summary>
/// Define operações para gerenciamento de schema de banco de dados, como criação e alteração de views.
/// </summary>
public interface ISchemaManagementService
{
    /// <summary>
    /// Cria ou altera uma view no banco de dados com base na definição SQL fornecida.
    /// </summary>
    /// <param name="connectionParameters">Parâmetros de conexão com o banco de dados.</param>
    /// <param name="viewName">Nome da view a ser criada ou alterada.</param>
    /// <param name="viewSqlDefinition">Definição SQL da view.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task<Result> CreateOrAlterViewAsync(ConnectionParameters connectionParameters, string viewName, string viewSqlDefinition); 

    /// <summary>
    /// Executa um comando SQL arbitrário no banco de dados.
    /// </summary>
    /// <param name="parameters">Parâmetros de conexão com o banco de dados.</param>
    /// <param name="sqlCommand">Comando SQL a ser executado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task ExecuteSqlCommandAsync(ConnectionParameters parameters, string sqlCommand);
}
