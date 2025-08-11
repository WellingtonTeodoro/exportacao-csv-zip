using Exportador.Core.ValueObjects;

namespace Exportador.Application.Interfaces
{
    /// <summary>
    /// Interface para serviços que realizam testes de conexão com banco de dados.
    /// </summary>
    public interface IDatabaseConnectionService
    {
        /// <summary>
        /// Testa a conexão com o banco de dados utilizando os parâmetros fornecidos.
        /// </summary>
        /// <param name="connectionParameters">Objeto de valor contendo os parâmetros da conexão.</param>
        /// <returns>Objeto de valor contendo o resultado do teste de conexão (sucesso/falha e mensagem).</returns>
        Task<ConnectionTestResult> TestConnectionAsync(ConnectionParameters connectionParameters); 
    }
}