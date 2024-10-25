using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace MyTemplate.Application.ApplicationManagement.Repositories.EF;

public interface IEFGenericRepository<TModel, TID> where TModel : BaseEntity<TID>, new()
{
    Task<bool> AnyAsync(Expression<Func<TModel, bool>> predicate, bool tracking = false, CancellationToken cancellationToken = default);

    Task<TModel?> FindAsync(TID id, bool tracking = false, CancellationToken cancellationToken = default);

    Task<TModel?> FirstOrDefaultAsync(Expression<Func<TModel, bool>> predicate, bool tracking = false);

    IQueryable<TModel> GetAll(Expression<Func<TModel, bool>>? predicate = null, bool tracking = false);

    Task<IList<TType>> GetAllAsync<TType>(Expression<Func<TModel, TType>> select,
                                                  Expression<Func<TModel, bool>>? predicate = null,
                                                  Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
                                                  Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>>? include = null,
                                                  bool tracking = false,
                                                  CancellationToken cancellationToken = default
        ) where TType : class;

    Task<TType?> GetAsync<TType>(Expression<Func<TModel, TType>> select,
                                                  Expression<Func<TModel, bool>>? predicate = null,
                                                  Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
                                                  Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>>? include = null,
                                                  bool tracking = false,
                                                  CancellationToken cancellationToken = default
        ) where TType : class;

    Task AddAsync(TModel entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TModel> entities, CancellationToken cancellationToken = default);

    void RemoveById(TID id);

    void Remove(TModel entity);

    void RemoveRange(IEnumerable<TModel> entities);

    void Update(TModel entity);

    void UpdateRange(IList<TModel> entities);

    void Update<TType>(TModel entity, Expression<Func<TModel, TType>> select) where TType : class;
}