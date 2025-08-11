using System.Data;
using Exportador.Application.Interfaces;
using Microsoft.Data.SqlClient;

namespace Exportador.Infrastructure.Services;

/// <summary>
/// Fábrica de conexões baseada em SQL Server.
/// Implementa <see cref="IDbConnectionFactory"/>.
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private readonly ILogService _logService;  

    /// <summary>
    /// Inicializa uma nova fábrica com a string de conexão especificada.
    /// </summary>
    /// <param name="connectionString">String de conexão do SQL Server.</param>
    /// <param name="logService">Serviço de log para registrar informações e erros.</param>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="connectionString"/> for nula ou vazia, ou se logService for nulo.</exception>
    public DbConnectionFactory(string connectionString, ILogService logService)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "A string de conexão não pode ser nula ou vazia.");
        }
        _connectionString = connectionString;
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _logService.Info("DbConnectionFactory inicializado.");
    }

    /// <inheritdoc />
    public IDbConnection CreateConnection()
    { 
        _logService.Debug("Criando nova instância de SqlConnection.");
        return new SqlConnection(_connectionString);
    }
}