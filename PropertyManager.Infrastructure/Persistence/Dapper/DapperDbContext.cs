using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Exceptions;
using Dapper.Contrib.Extensions;

namespace PropertyManager.Infrastructure.Persistence.Dapper;

/// <summary>
/// Dapper implementation of the DbContext. This class is used to perform CRUD operations on the database.
/// </summary> 
public sealed class DapperDbContext : IDisposable
{
    #region Fields
    /// <summary>
    /// The connection object to database.
    /// </summary>
    private readonly IDbConnection _connection;
    /// <summary>
    /// The transaction object to database.
    /// </summary>
    private IDbTransaction _transaction = null!;
    /// <summary>
    /// The logger object to log the messages.
    /// </summary>
    private readonly ILogger<DapperDbContext> _logger;
    /// <summary>
    /// The command timeout.
    /// </summary>
    private readonly int? _commandTimeout = null;

    #endregion
    /// <summary>
    /// Dapper DbContext Constructor with IDbConnectionProvider that is injected by DI.
    /// </summary>
    /// <param name = "dbConnectionProvider"></param>
    /// <param name = "logger"></param>
    public DapperDbContext(IDbConnectionProvider dbConnectionProvider, ILogger<DapperDbContext> logger)
    {
        _connection = dbConnectionProvider.GetConnection();
        _connection.Open();
        _logger = logger;
    }

    #region Transaction
    /// <summary>
    /// Begins the transaction.
    /// </summary>
    public void BeginTransaction()
    {
        _transaction = _connection.BeginTransaction();
        _logger.LogInformation("Transaction started");
    }

    /// <summary>
    /// Commits the transaction.
    /// </summary>
    public void Commit()
    {
        _transaction.Commit();
        _logger.LogInformation("Transaction committed");
    }

    /// <summary>
    /// Rolls Back The Transaction.
    /// </summary>
    public void Rollback()
    {
        _transaction.Rollback();
        _logger.LogInformation("Transaction rolled backed");
    }

    #endregion Transaction
    #region Dapper.Contrib.Extensions
    /// <summary>
    /// Gets the entity by id
    /// </summary>
    /// <param name = "id"></param>
    /// <typeparam name = "T"></typeparam>
    /// <returns></returns>
    /// <exception cref = "ArgumentNullException"></exception>
    public async Task<T> GetAsync<T>(int id)
        where T : class
    {
        try
        {
            return await _connection.GetAsync<T>(id, _transaction, _commandTimeout);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting entity");
            throw;
        }
    }

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>()
        where T : class
    {
        try
        {
            return await _connection.GetAllAsync<T>(_transaction, _commandTimeout);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all entities");
            throw;
        }
    }

    /// <summary>
    /// Updates the entity
    /// </summary>
    /// <param name = "model"></param>
    /// <typeparam name = "T"></typeparam>
    /// <returns></returns>
    /// <exception cref = "ArgumentNullException"></exception>
    public async Task<bool> UpdateAsync<T>(T model)
        where T : class
    {
        try
        {
            return await _connection.UpdateAsync(model, _transaction, _commandTimeout);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating entity");
            throw;
        }
    }

    /// <summary>
    /// Inserts an entity asynchronously
    /// </summary>
    /// <param name = "entity"></param>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref = "ArgumentNullException"></exception>
    /// <exception cref = "DbContextException"></exception>
    public async Task<int> InsertAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        try
        {
            var result = await _connection.InsertAsync(entity, _transaction);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while bulk Inserting Entities {@Entity}", nameof(TEntity));
            throw new DbContextException(ex.Message, ex);
        }
    }


    /// <summary>
    /// Insert an entity asynchronously
    /// </summary>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns>1 if successful, 0 otherwise</returns>
    /// <exception cref = "ArgumentNullException"> Thrown when TEntity is null</exception>
    /// <exception cref = "DbContextException">Thrown when an error occurs deleting the Entity</exception>
    public async Task<int> InsertCustomAsync<TEntity>(string query, Dictionary<string, object?>? parameters = null)
        where TEntity : class
    {
        try
        {
            TableNameHelper.GetTableName<TEntity>();

            var result = await _connection.ExecuteAsync(query, parameters, _transaction);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while executing save async on Entity {@Entity}", nameof(TEntity));
            throw new DbContextException(ex.Message, ex);
        }
    }

    #endregion
    #region Custom  Execute & Qeury
    /// <summary>
    /// Gets a single record from the database.
    /// </summary>
    /// <param name = "where"></param>
    /// <param name = "parameters"></param>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns></returns>
    public async Task<TEntity> GetFirstOrDefaultAsync<TEntity>(string? where = null, Dictionary<string, object?>? parameters = null)
        where TEntity : class
    {
        var tableName = TableNameHelper.GetTableName<TEntity>();
        var query = $"select * from  {tableName} {(string.IsNullOrWhiteSpace(where) ? "" : $"WHERE {where}")}";
        try
        {
            return await _connection.QueryFirstOrDefaultAsync<TEntity>(query, new DynamicParameters(parameters), _transaction);
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Custom query that returns a single record from the database.
    /// </summary>
    /// <param name = "query"></param>
    /// <param name = "parameters"></param>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns></returns>
    public async Task<TEntity> CustomSingleQueryAsync<TEntity>(string query, Dictionary<string, object?>? parameters = null)
    {
        try
        {
            return await _connection.QueryFirstOrDefaultAsync<TEntity>(query, new DynamicParameters(parameters), _transaction);
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Custom query that returns a list of records from the database.
    /// </summary>
    /// <param name = "query"></param>
    /// <param name = "parameters"></param>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> CustomQueryListAsync<TEntity>(string query, Dictionary<string, object?>? parameters = null)
    {
        try
        {
            return await _connection.QueryAsync<TEntity>(query, new DynamicParameters(parameters), _transaction)!;
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Gets a list of records from the database.
    /// </summary>
    /// <param name = "where"></param>
    /// <param name = "parameters"></param>
    /// <param name = "orderBy"></param>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(string? where = null, Dictionary<string, object?>? parameters = null, string? orderBy = null)
    {
        var tableName = TableNameHelper.GetTableName<TEntity>();

        var query = $"SELECT * FROM {tableName} {(string.IsNullOrWhiteSpace(where) ? "" : $"WHERE {where}")} {(string.IsNullOrWhiteSpace(orderBy) ? "" : $"ORDER BY {orderBy}")}";
        try
        {
            return await _connection.QueryAsync<TEntity>(query, new DynamicParameters(parameters), _transaction);
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Delete an entity asynchronously
    /// </summary>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns>1 if successful, 0 otherwise</returns>
    /// <exception cref = "ArgumentNullException"> Thrown when TEntity is null</exception>
    /// <exception cref = "DbContextException">Thrown when an error occurs deleting the Entity</exception>
    public async Task<int> DeleteAsync<TEntity>(string? where = null, Dictionary<string, object?>? parameters = null)
        where TEntity : class
    {
        try
        {
            var tableName = TableNameHelper.GetTableName<TEntity>();
            var query = $"DELETE FROM {tableName} {(string.IsNullOrWhiteSpace(where) ? "" : $"WHERE {where}")}";
            return await _connection.ExecuteAsync(query, parameters, _transaction);
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Executes the stored procedure and returns a list of entities.
    /// </summary>
    /// <typeparam name = "TEntity">Type of entity to return.</typeparam>
    /// <param name = "storedProcedureName">Name of stored procedure to execute.</param>
    /// <param name = "parameters">Optional parameters for stored procedure.</param>
    /// <returns>List of entities returned by stored procedure.</returns>
    /// <exception cref = "DbContextException">Thrown when an error occurs executing the stored procedure.</exception>
    public async Task<IEnumerable<TEntity>> ExecuteSpListAsync<TEntity>(string storedProcedureName, Dictionary<string, object?>? parameters = null)
    {
        try
        {
            return await _connection.QueryAsync<TEntity>(storedProcedureName, parameters, _transaction, _commandTimeout, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Executes an asynchronous stored procedure with the specified parameters.
    /// </summary>
    /// <param name = "storedProcedureName">The name of the stored procedure to execute.</param>
    /// <param name = "parameters">The parameters for the stored procedure.</param>
    /// <returns>The number of rows affected by the stored procedure execution.</returns>
    /// <exception cref = "ArgumentNullException">Thrown when the storedProcedureName is null or empty.</exception>
    /// <exception cref = "DbContextException">Thrown when an error occurs while executing the stored procedure.</exception>
    public async Task<int> ExecuteSpAsync(string storedProcedureName, Dictionary<string, object?>? parameters = null)
    {
        try
        {
            return await _connection.ExecuteAsync(storedProcedureName, parameters, _transaction, _commandTimeout, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }


    public async Task<SqlMapper.GridReader> QueryMultipleAsync(
        string sql,
        Dictionary<string, object?>? parameters = null)
    {

        try
        {
            return await _connection.QueryMultipleAsync(sql, parameters, _transaction, _commandTimeout, CommandType.Text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while executing QueryMultipleAsync");
            throw new DbContextException(ex.Message, ex);
        }
    }

    #endregion

    #region IDisposable
    /// <summary>
    /// Disposes the 
    /// </summary>
    public void Dispose()
    {
        _transaction.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
    #endregion
}
