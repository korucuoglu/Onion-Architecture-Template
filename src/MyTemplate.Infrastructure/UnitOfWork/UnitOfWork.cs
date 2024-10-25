using MyTemplate.Application.ApplicationManagement.Repositories.EF;
using MyTemplate.Application.ApplicationManagement.Repositories.UnitOfWork;

namespace MyTemplate.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEFUnitOfWork EF => new EFUnitOfWork(_context);
}