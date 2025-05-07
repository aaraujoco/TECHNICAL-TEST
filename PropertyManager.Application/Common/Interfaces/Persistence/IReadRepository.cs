namespace PropertyManager.Application.Common.Interfaces.Persistence;

/// <summary>
/// Interface for the read repository that is used to retrieve data from the database.
/// </summary>
/// <typeparam name = "TEntity"></typeparam>
public interface IReadRepository<TEntity> : IDisposable where TEntity : class, new()
{
    /// <summary>
    /// Gets a single record from the database by id.
    /// </summary>
    Task<TEntity> GetAsync(int id);
    /// <summary>
    /// Gets a single record from the database.
    /// query is used to filter the data.
    /// parameters is used to pass parameters to the query.
    /// </summary>
    /// <param name = "query"></param>
    /// <param name = "parameters"></param>
    /// <returns></returns>
    Task<TEntity> CustomSingleQueryAsync(string query, Dictionary<string, object?>? parameters = null);
    /// <summary>
    /// Gets a list of entities from the database.
    /// </summary>
    /// <param name = "query"></param>
    /// <param name = "parameters"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> CustomSingleQueryListAsync(string query, Dictionary<string, object?>? parameters = null);
    /// <summary>
    /// 
    /// </summary>
    /// <param name = "where"></param>
    /// <param name = "parameters"></param>
    /// <returns></returns>
    Task<TEntity> GetFirstOrDefaultAsync(string? where = null, Dictionary<string, object?>? parameters = null);
    /// <summary>
    /// 
    /// </summary>
    /// <param name = "where"></param>
    /// <param name = "parameters"></param>
    /// <param name = "orderBy"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetListAsync(string? where = null, Dictionary<string, object?>? parameters = null, string? orderBy = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "storedProcedureName"></param>
    /// <param name = "parameters"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> ExecuteSpListAsync(string storedProcedureName, Dictionary<string, object?>? parameters = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "storedProcedureName"></param>
    /// <param name = "parameters"></param>
    /// <returns></returns>
    Task<int> ExecuteSpAsync(string storedProcedureName, Dictionary<string, object?>? parameters = null);

    Task<(List<TEntity>, int)> QueryMultipleAsync(string sql, Dictionary<string, object?>? parameters = null);
}
