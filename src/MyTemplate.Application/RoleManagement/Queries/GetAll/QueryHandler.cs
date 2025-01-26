using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyTemplate.Application.RoleManagement.Queries.GetAll;

public class QueryHandler : QueryHandlerBase<Query, IEnumerable<Dto>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public QueryHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }


    protected override async Task<Result<IEnumerable<Dto>>> HandleAsync(Query request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles.ToListAsync(cancellationToken);

        var models = roles.Select(x => new Dto()
        {
            Id = x.Id,
            Name = x.Name ?? "",
        });

        return Result<IEnumerable<Dto>>.WithSuccess(models);
    }
}