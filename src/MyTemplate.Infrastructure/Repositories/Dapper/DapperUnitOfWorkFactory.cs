using MyTemplate.Application.ApplicationManagement.Repositories.Dapper;

namespace MyTemplate.Infrastructure.Repositories.Dapper;

/// <summary>
///
/// </summary>
public abstract class DapperUnitOfWorkFactory<T> : IDapperUnitOfWorkFactory<T> where T : IDapperUnitOfWork<IDapperDbContext>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public abstract T Create();
}