using System.Data;
using Common.Repositories.Dapper;

namespace MyTemplate.Infrastructure.Repositories.Dapper;

public abstract class DapperUnitOfWork<T> : IDapperUnitOfWork<T> where T : class, IDapperDbContext
{
    public T Context { get; }

    protected DapperUnitOfWork(T context)
    {
        Context = context;
    }

    public virtual void OpenConnection()
    {
        if (Context.Connection is { State: ConnectionState.Closed })
        {
            Context.Connection.Open();
        }
    }

    public virtual void CloseConnection()
    {
        if (Context.Connection is not { State: ConnectionState.Closed })
        {
            Context.Connection?.Close();
        }
    }

    public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        Context.Transaction = Context.Connection?.BeginTransaction(isolationLevel);
    }

    public virtual void CommitTransaction()
    {
        if (Context.Transaction is not null)
        {
            try
            {
                Context.Transaction.Commit();
            }
            catch
            {
                Context.Transaction.Rollback();

                throw;
            }
        }
    }

    public virtual void RollbackTransaction()
    {
        if (Context.Transaction is not null)
        {
            Context.Transaction.Rollback();
        }
    }

    public virtual void Dispose()
    {
        if (Context.Transaction != null)
        {
            Context.Transaction.Dispose();
            Context.Transaction = null;
        }

        if (Context.Connection == null) return;

        if (Context.Connection.State == (ConnectionState.Open | ConnectionState.Executing | ConnectionState.Fetching)) Context.Connection.Close();

        Context.Connection.Dispose();
        Context.Connection = null;
    }
}