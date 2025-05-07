using PropertyManager.Application.Common.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Infrastructure.Persistence.Dapper;

/// <summary>
/// Repository class for writing data to the database using Dapper. Implements `IWriteRepository` and `IBulkRepository` interfaces.
/// </summary>
/// <typeparam name = "TEntity">The type of entity to write to the database.</typeparam>
public sealed class DapperWriteRepository<TEntity> : IWriteRepository<TEntity> where TEntity : class, new()
{
    private readonly DapperDbContext _dbContext;

    /// <summary>
    /// Constructor that takes in a `DapperDbContext` object.
    /// </summary>
    /// <param name = "dataContext">The `DapperDbContext` object to be used for database operations.</param>
    public DapperWriteRepository(DapperDbContext dataContext) => _dbContext = dataContext;

    /// <summary>
    /// Asynchronously executes a stored procedure with the given name and parameters.
    /// </summary>
    /// <param name = "storedProcedureName">The name of the stored procedure to execute.</param>
    /// <param name = "parameters">The parameters to be passed to the stored procedure. Optional.</param>
    /// <returns>The number of rows affected by the execution of the stored procedure.</returns>
    public Task<int> ExecuteSpAsync(string storedProcedureName, Dictionary<string, object?>? parameters = null) =>
        string.IsNullOrWhiteSpace(storedProcedureName)
            ? throw new ArgumentNullException(nameof(storedProcedureName))
            : _dbContext.ExecuteSpAsync(storedProcedureName, parameters);

    /// <summary>
    /// Asynchronously updates an entity in the database.
    /// </summary>
    /// <param name = "entity">The entity to be updated in the database.</param>
    /// <returns>A boolean indicating the success of the update operation.</returns>
    public async Task<bool> UpdateAsync(TEntity? entity) => entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : await _dbContext.UpdateAsync(entity);

    /// <summary>
    /// Asynchronously deletes an entity from the database.
    /// </summary>
    /// <returns>A boolean indicating the success of the delete operation.</returns>
    public async Task<int> DeleteAsync(string query, Dictionary<string, object?>? parameters = null) =>
        await _dbContext.DeleteAsync<TEntity>(query, parameters);

    /// <summary>
    /// Asynchronously inserts an entity into the database.
    /// </summary>
    /// <param name = "entity">The entity to be inserted into the database.</param>
    /// <returns>The number of rows affected by the insertion operation.</returns>
    public Task<int> InsertAsync(TEntity entity) => entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : _dbContext.InsertAsync(entity);

    /// <summary>
    /// Disposes the database context.
    /// </summary>
    public void Dispose() => _dbContext.Dispose();

    public async Task<int> InsertCustomAsync(string query, Dictionary<string, object?>? parameters = null) => await _dbContext.InsertCustomAsync<TEntity>(query, parameters);

}
