using System.Data;

namespace MyTemplate.Infrastructure.Repositories.Dapper;

/// <summary>
///
/// </summary>
public abstract class DapperRepositoryBase
{
    protected readonly IDbConnection DbConnection;

    protected readonly IDbTransaction DbTransaction;

    protected readonly int CommandTimeout;

    /// <summary>
    ///
    /// </summary>
    /// <param name="dbConnection"></param>
    /// <param name="dbTransaction"></param>
    /// <param name="commandTimeout"></param>
    protected DapperRepositoryBase(IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeout = 30)
    {
        DbConnection = dbConnection;
        DbTransaction = dbTransaction;
        CommandTimeout = commandTimeout;
    }
}