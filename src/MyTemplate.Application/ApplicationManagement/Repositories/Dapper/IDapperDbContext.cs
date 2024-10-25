using System.Data;

namespace MyTemplate.Application.ApplicationManagement.Repositories.Dapper;

public interface IDapperDbContext
{
    IDbConnection? Connection { get; set; }
    IDbTransaction? Transaction { get; set; }
}