using System.Data;

namespace Exportador.Application.Interfaces
{
    /// <summary>
    /// Interface para abstração de criação de conexões com banco de dados.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Cria e retorna uma nova instância de conexão com o banco.
        /// </summary>
        /// <returns>Instância de <see cref="IDbConnection"/>.</returns>
        IDbConnection CreateConnection();
    }
}
