namespace MyTemplate.Application.ApplicationManagement.Repositories.EF;

public interface IEFUnitOfWork : IAsyncDisposable
{
    IEFGenericRepository<TModel, TId> GetRepository<TModel, TId>() where TModel : BaseEntity<TId>, new();

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task RollBackAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}