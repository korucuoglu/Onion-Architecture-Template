using Common.Interfaces.Repositories.EF;
using MyTemplate.Domain.Interfaces;
using MyTemplate.Infrastructure.Context;

namespace MyTemplate.Infrastructure.EF;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEFUnitOfWork EF => new EFUnitOfWork(_context);
}