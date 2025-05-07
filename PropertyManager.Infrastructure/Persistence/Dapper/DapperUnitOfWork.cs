using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;

namespace PropertyManager.Infrastructure.Persistence.Dapper;

/// <summary>
/// DapperUnitOfWork class is responsible for managing the database transaction and providing repository instances for read and write operations.
/// The class uses dependency injection to resolve the repositories, and it keeps track of them in a ConcurrentDictionary to avoid creating multiple instances of the same repository.
/// It also has methods for committing or rolling back the transaction and disposing the resources.
/// </summary>s
public sealed class DapperUnitOfWork : IUnitOfWork
{
    private readonly DapperDbContext _dbContext;
    private readonly ILogger<DapperUnitOfWork> _logger;
    private readonly ConcurrentDictionary<Type, object> _readRepositories = new();
    /// <summary>
    /// constructor which initializes the DbContext and starts a new transaction
    /// </summary>
    /// <param name = "dbContext"></param>
    /// <param name = "logger"></param>
    public DapperUnitOfWork(DapperDbContext dbContext, ILogger<DapperUnitOfWork> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dbContext.BeginTransaction();
    }

    /// <summary>
    /// returns a read repository instance for a specific entity type
    /// </summary>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns></returns>
    public IReadRepository<TEntity> GetReadRepository<TEntity>()
        where TEntity : class, new()
    {
        var type = typeof(TEntity);
        if (!_readRepositories.ContainsKey(type))
        {
            _readRepositories[type] = new DapperReadRepository<TEntity>(_dbContext);
        }

        return (IReadRepository<TEntity>)_readRepositories[type];
    }

    /// <summary>
    /// commits the transaction if no error occurs, 
    /// otherwise rollback the transaction and return false
    /// </summary>
    /// <returns></returns>
    public bool SaveChanges()
    {
        try
        {
            _dbContext.Commit();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database");
            _dbContext.Rollback();
        }

        return false;
    }

    /// <summary>
    /// disposes the resources
    /// </summary>
    public void Dispose()
    {
        _dbContext.Dispose();
        _readRepositories.Clear();
    }
}