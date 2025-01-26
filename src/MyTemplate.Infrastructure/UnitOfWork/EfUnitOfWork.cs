using Common.Interfaces.Repositories.Ef;
using Common.Services.Repositories.Ef;

namespace MyTemplate.Infrastructure.UnitOfWork;

public class EfUnitOfWork : IEfUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public EfUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default) => await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task RollBackAsync(CancellationToken cancellationToken = default) => await _context.Database.RollbackTransactionAsync(cancellationToken);

    public async Task CommitAsync(CancellationToken cancellationToken = default) => await _context.Database.CommitTransactionAsync(cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);

    IEfGenericRepository<TModel, TId> IEfUnitOfWork.GetRepository<TModel, TId>()
    {
        return new EfGenericRepository<TModel, TId, ApplicationDbContext>(_context);
    }
}