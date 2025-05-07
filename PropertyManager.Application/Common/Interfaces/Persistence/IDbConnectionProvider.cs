using System.Data;

namespace PropertyManager.Application.Common.Interfaces.Persistence;

/// <summary>
/// Interface for the database connection provider
/// </summary>
public interface IDbConnectionProvider
{
    /// <summary>
    /// Gets the database connection
    /// </summary>
    /// <returns></returns>
    IDbConnection GetConnection();
}
