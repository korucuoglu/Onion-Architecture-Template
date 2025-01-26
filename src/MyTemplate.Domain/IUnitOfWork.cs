using Common.Interfaces.Repositories.Ef;

namespace MyTemplate.Application.ApplicationManagement.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    public IEfUnitOfWork EF { get; }
    
    // public IDapperUnitOfWork<IDapperDbContext> Dapper { get; }
}