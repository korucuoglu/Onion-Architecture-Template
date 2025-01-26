using Common.Repositories.Ef;
using MyTemplate.Application.ApplicationManagement.Repositories.UnitOfWork;

namespace MyTemplate.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEfUnitOfWork EF => new EfUnitOfWork(_context);
}