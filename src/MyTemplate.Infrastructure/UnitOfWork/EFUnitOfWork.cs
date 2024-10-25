using MyTemplate.Application.ApplicationManagement.Repositories.EF;
using MyTemplate.Infrastructure.Repositories.EF;

namespace MyTemplate.Infrastructure.UnitOfWork;

public class EFUnitOfWork : IEFUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public EFUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default) => await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task RollBackAsync(CancellationToken cancellationToken = default) => await _context.Database.RollbackTransactionAsync(cancellationToken);

    public async Task CommitAsync(CancellationToken cancellationToken = default) => await _context.Database.CommitTransactionAsync(cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);

    IEFGenericRepository<TModel, TId> IEFUnitOfWork.GetRepository<TModel, TId>()
    {
        return new EFGenericRepository<TModel, TId, ApplicationDbContext>(_context);
    }
}