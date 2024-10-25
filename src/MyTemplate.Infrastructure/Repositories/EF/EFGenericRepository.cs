using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using MyTemplate.Application.ApplicationManagement.Repositories.EF;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Infrastructure.Repositories.EF;

public class EFGenericRepository<TModel, TId, TContext> : IEFGenericRepository<TModel, TId>
    where TModel : BaseEntity<TId>, new()
    where TContext : DbContext

{
    private readonly DbSet<TModel> _table;
    private readonly TContext _context;

    public EFGenericRepository(TContext context)
    {
        _context = context;
        _table = context.Set<TModel>();
    }

    //private static readonly Func<TContext, Expression<Func<TModel, bool>>, Task<TModel?>> FirstAsync =
    //    Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((TContext context, Expression<Func<TModel, bool>> predicate) =>
    //   context.Set<TModel>().FirstOrDefault(predicate));

    //private static readonly Func<TContext, Expression<Func<TModel, bool>>, IEnumerable<TModel>> CompileWhere =
    //Microsoft.EntityFrameworkCore.EF.CompileQuery((TContext context, Expression<Func<TModel, bool>> predicate) =>
    //    context.Set<TModel>().Where(predicate));

    public async Task<TModel?> FirstOrDefaultAsync(Expression<Func<TModel, bool>> predicate, bool tracking = false)
    {
        IQueryable<TModel> query = _table;
        if (!tracking) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<TModel?> FindAsync(TId id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var entity = await _table.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

        if (entity is not null && !tracking)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<bool> AnyAsync(Expression<Func<TModel, bool>> predicate, bool tracking = false, CancellationToken cancellationToken = default)
    {
        IQueryable<TModel> query = _table;
        if (!tracking) query = query.AsNoTracking();
        return await query.AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<TModel> GetAll(Expression<Func<TModel, bool>>? predicate = null, bool tracking = false)
    {
        IQueryable<TModel> query = _table;
        if (!tracking) query = query.AsNoTracking();
        return predicate == null ? query : query.Where(predicate);
    }

    public async Task<IList<TType>> GetAllAsync<TType>(Expression<Func<TModel, TType>> select,
                                                  Expression<Func<TModel, bool>>? predicate = null,
                                                  Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
                                                  Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>>? include = null,
                                                  bool tracking = false,
                                                  CancellationToken cancellationToken = default
        ) where TType : class
    {
        IQueryable<TModel> query = _table;
        if (!tracking) query = query.AsNoTracking();
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null) query = orderBy(query);
        return await query.Select(select).ToListAsync(cancellationToken);
    }

    public async Task<TType?> GetAsync<TType>(Expression<Func<TModel, TType>> select,
                                                  Expression<Func<TModel, bool>>? predicate = null,
                                                  Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
                                                  Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>>? include = null,
                                                  bool tracking = false,
                                                  CancellationToken cancellationToken = default
        ) where TType : class
    {
        IQueryable<TModel> query = _table;
        if (!tracking) query = query.AsNoTracking();
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null) query = orderBy(query);
        return await query.Select(select).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(TModel entity, CancellationToken cancellationToken = default)
    {
        await _table.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TModel> entities, CancellationToken cancellationToken = default)
    {
        await _table.AddRangeAsync(entities, cancellationToken);
    }

    public void Remove(TModel entity)
    {
        _table.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TModel> entities)
    {
        _table.RemoveRange(entities);
    }

    public void RemoveById(TId id)
    {
        _table.Remove(new() { Id = id });
    }

    public void Update<TType>(TModel entity, Expression<Func<TModel, TType>> select) where TType : class
    {
        _context.Entry(entity).State = EntityState.Unchanged;

        typeof(TType).GetProperties().ToList().ForEach(p =>
        {
            _context.Entry(entity).Property(p.Name).IsModified = true;
        });
    }

    public void Update(TModel entity)
    {
        _context.Update(entity);
    }

    public void UpdateRange(IList<TModel> entities)
    {
        _context.UpdateRange(entities);
    }
}