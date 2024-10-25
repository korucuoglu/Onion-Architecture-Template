namespace MyTemplate.Application.ApplicationManagement.Repositories.Dapper;

public interface IDapperUnitOfWorkFactory<out T> where T : IDapperUnitOfWork<IDapperDbContext>
{
    T Create();
}