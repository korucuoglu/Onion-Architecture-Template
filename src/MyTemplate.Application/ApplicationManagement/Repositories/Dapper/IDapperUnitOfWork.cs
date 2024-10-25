using System.Data;

namespace MyTemplate.Application.ApplicationManagement.Repositories.Dapper;

public interface IDapperUnitOfWork<out T> : IDisposable where T : IDapperDbContext
{
    T Context { get; }

    void OpenConnection();

    void CloseConnection();

    void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    void CommitTransaction();

    void RollbackTransaction();
}