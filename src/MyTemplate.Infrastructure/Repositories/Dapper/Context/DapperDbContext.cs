using System.Data;
using MyTemplate.Application.ApplicationManagement.Repositories.Dapper;

namespace MyTemplate.Infrastructure.Repositories.Dapper.Context;

public abstract class DapperDbContext : IDapperDbContext
{
    public IDbConnection? Connection { get; set; }
    public IDbTransaction? Transaction { get; set; }
    public int CommandTimeout { get; set; }
}