using Exportador.Core.ValueObjects;
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;  

namespace Exportador.Infrastructure.Helpers;

/// <summary>
/// Classe auxiliar para construir strings de conexão SQL Server de forma segura.
/// </summary>
public static class SqlConnectionStringBuilderHelper
{
    /// <summary>
    /// Constrói uma string de conexão SQL Server a partir de um objeto <see cref="ConnectionParameters"/>.
    /// Garante o manuseio seguro da senha utilizando SecureString.
    /// </summary>
    /// <param name="parameters">O objeto de valor ConnectionParameters contendo os detalhes da conexão.</param>
    /// <param name="trustServerCertificate">Indica se o certificado do servidor deve ser confiado sem validação.
    /// Deve ser 'true' para certificados autoassinados ou 'false' para certificados de CA válidas em produção.</param>
    /// <param name="connectTimeout">O tempo limite da conexão em segundos.</param>
    /// <returns>A string de conexão construída.</returns>
    /// <exception cref="ArgumentNullException">Lançada se <paramref name="parameters"/> for nulo.</exception>
    public static string Build(ConnectionParameters parameters, bool trustServerCertificate, int connectTimeout = 10)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters), "Os parâmetros de conexão não podem ser nulos.");

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = parameters.Server,
            InitialCatalog = parameters.Database,
            UserID = parameters.User,
            TrustServerCertificate = trustServerCertificate, 
            ConnectTimeout = connectTimeout
        };
         
        IntPtr bstrPtr = IntPtr.Zero;
        string? password = null;  
        try
        {
            if (parameters.Password != null && parameters.Password.Length > 0)
            {
                bstrPtr = Marshal.SecureStringToBSTR(parameters.Password);
                password = Marshal.PtrToStringBSTR(bstrPtr);
                builder.Password = password;
            }
        }
        finally
        { 
            if (bstrPtr != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstrPtr);
            } 
        }

        return builder.ConnectionString;
    }
}