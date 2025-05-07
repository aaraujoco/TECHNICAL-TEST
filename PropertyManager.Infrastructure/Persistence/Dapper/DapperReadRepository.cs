using PropertyManager.Application.Common.Interfaces.Persistence;

namespace PropertyManager.Infrastructure.Persistence.Dapper;

/// <summary>
/// Read Repository for Dapper
/// </summary>
/// <typeparam name = "TEntity"></typeparam>
public sealed class DapperReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, new()
{
    private readonly DapperDbContext _dbContext;
    /// <summary>
    /// Initializes a new instance of the <see cref = "DapperReadRepository{TEntity}"/> class.
    /// </summary>
    /// <param name = "dataContext">The Dapper database context</param>
    public DapperReadRepository(DapperDbContext dataContext) => _dbContext = dataContext;
    /// <summary>
    /// Executes a custom query to retrieve a single entity
    /// </summary>
    /// <param name = "query">The query to execute</param>
    /// <param name = "parameters">Optional parameters to use with the query</param>
    /// <returns>The retrieved entity</returns>
    public Task<TEntity> CustomSingleQueryAsync(string query, Dictionary<string, object?>? parameters = null) => string.IsNullOrWhiteSpace(query) ? throw new ArgumentNullException(nameof(query)) : _dbContext.CustomSingleQueryAsync<TEntity>(query, parameters);
    /// <summary>
    /// Executes a custom single query for a list of entities
    /// </summary>
    /// <param name = "query">The query to be executed</param>
    /// <param name = "parameters">The parameters for the query, if any</param>
    /// <returns>A Task that returns a list of entities</returns>
    public Task<IEnumerable<TEntity>> CustomSingleQueryListAsync(string query, Dictionary<string, object?>? parameters = null) => string.IsNullOrWhiteSpace(query) ? throw new ArgumentNullException(nameof(query)) : _dbContext.CustomQueryListAsync<TEntity>(query, parameters);
    /// <summary>
    /// Retrieves the first or default entity matching the given conditions
    /// </summary>
    /// <param name = "where">The conditions to match</param>
    /// <param name = "parameters">Optional parameters to use with the conditions</param>
    /// <returns>The first or default matching entity</returns>
    public Task<TEntity> GetFirstOrDefaultAsync(string? where = null, Dictionary<string, object?>? parameters = null) => _dbContext.GetFirstOrDefaultAsync<TEntity>(where, parameters);
    /// <summary>
    /// Executes a stored procedure to retrieve a list of entities
    /// </summary>
    /// <param name = "storedProcedureName">The name of the stored procedure to execute</param>
    /// <param name = "parameters">Optional parameters to use with the stored procedure</param>
    /// <returns>The retrieved list of entities</returns>
    public Task<IEnumerable<TEntity>> ExecuteSpListAsync(string storedProcedureName, Dictionary<string, object?>? parameters = null) => string.IsNullOrWhiteSpace(storedProcedureName) ? throw new ArgumentNullException(nameof(storedProcedureName)) : _dbContext.ExecuteSpListAsync<TEntity>(storedProcedureName, parameters);

    /// <summary>
    /// Executes a stored procedure to retrieve a entities
    /// </summary>
    /// <param name = "storedProcedureName">The name of the stored procedure to execute</param>
    /// <param name = "parameters">Optional parameters to use with the stored procedure</param>
    /// <returns>The retrieved list of entities</returns>
    public Task<int> ExecuteSpAsync(string storedProcedureName, Dictionary<string, object?>? parameters = null) => string.IsNullOrWhiteSpace(storedProcedureName) ? throw new ArgumentNullException(nameof(storedProcedureName)) : _dbContext.ExecuteSpAsync(storedProcedureName, parameters);

    /// <summary>
    /// Gets a list of entities based on a where clause and optional order by clause.
    /// </summary>
    /// <param name = "where">The where clause used to filter the entities. Default is null.</param>
    /// <param name = "parameters">The parameters used in the where clause. Default is null.</param>
    /// <param name = "orderBy">The order by clause used to sort the entities. Default is null.</param>
    /// <returns>A task that returns a collection of entities.</returns>

    public async Task<IEnumerable<TEntity>> GetListAsync(string? where = null, Dictionary<string, object?>? parameters = null, string? orderBy = null) => await _dbContext.GetListAsync<TEntity>(where, parameters, orderBy);
    /// <summary>
    /// Gets a single entity by its id.
    /// </summary>
    /// <param name = "id">The id of the entity to retrieve.</param>
    /// <returns>A task that returns a single entity.</returns>
    public async Task<TEntity> GetAsync(int id) => id > -1 ? await _dbContext.GetAsync<TEntity>(id) : throw new ArgumentNullException(nameof(id));


    public async Task<(List<TEntity>, int)> QueryMultipleAsync(string sql,
        Dictionary<string, object?>? parameters = null)
    {
        var result = await _dbContext.QueryMultipleAsync(sql, parameters);

        return (result.Read<TEntity>().ToList(), result.Read<int>().FirstOrDefault());
    }

    /// <summary>
    /// Disposes the context.
    /// </summary>
    public void Dispose() => _dbContext.Dispose();

   
}
