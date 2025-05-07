namespace PropertyManager.Application.Common.Interfaces.Persistence;

/// <summary>
/// Interface for the write repository
/// </summary>
/// <typeparam name = "TEntity"></typeparam>
public interface IWriteRepository<in TEntity> : IDisposable where TEntity : class, new()
{
    /// <summary>
    /// Inserts an entity into the database using stored procedure.
    /// </summary>
    /// <param name = "storedProcedureName"></param>
    /// <param name = "parameters"></param>
    /// <returns></returns>
    Task<int> ExecuteSpAsync(string storedProcedureName, Dictionary<string, object?>? parameters = null);
    /// <summary>
    /// Updates an entity in the database.
    /// </summary>
    /// <param name = "entity"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(TEntity entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name = "query"></param>
    /// <param name = "parameters"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(string query, Dictionary<string, object?>? parameters = null);
    /// <summary>
    /// Asynchronously inserts an entity into the database.
    /// </summary>
    /// <param name = "entity">The entity to be inserted into the database.</param>
    /// <returns>The number of rows affected by the insertion operation.</returns>
    Task<int> InsertAsync(TEntity entity);
}
