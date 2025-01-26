using Common.Repositories.EF;

namespace MyTemplate.Application.ApplicationManagement.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    public IEFUnitOfWork EF { get; }
    
    // public IDapperUnitOfWork<IDapperDbContext> Dapper { get; }
}