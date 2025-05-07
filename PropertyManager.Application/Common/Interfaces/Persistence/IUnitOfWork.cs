namespace PropertyManager.Application.Common.Interfaces.Persistence;

/// <summary>
/// Interface for the unit of work that is used to save changes to the database.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the read repository and creates it if it doesn't exist.
    /// </summary>
    /// <typeparam name = "TEntity"></typeparam>
    /// <returns></returns>
    IReadRepository<TEntity> GetReadRepository<TEntity>()
        where TEntity : class, new();
    /// <summary>
    /// Commits the changes to the database.
    /// </summary>
    bool SaveChanges();
}
