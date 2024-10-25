using MyTemplate.Application.ApplicationManagement.Repositories.Dapper;
using Npgsql;

namespace MyTemplate.Infrastructure.Repositories.Dapper.Context;

public interface IPostgreSqlDbContext : IDapperDbContext
{
}

public class PostgreSqlDbContext : DapperDbContext, IPostgreSqlDbContext
{
    public PostgreSqlDbContext(string connectionString, int commandTimeout)
    {
        Connection = new NpgsqlConnection(connectionString);
        CommandTimeout = commandTimeout;
    }
}