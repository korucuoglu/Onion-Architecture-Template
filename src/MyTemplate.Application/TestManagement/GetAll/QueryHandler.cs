using MyTemplate.Application.ApplicationManagement.Repositories.UnitOfWork;

namespace MyTemplate.Application.TestManagement.GetAll;

public class QueryHandler : QueryHandlerBase<Query, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public QueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<int>> HandleAsync(Query request, CancellationToken cancellationToken)
    {
        var data = await Task.FromResult(55);
        
        return Result<int>.WithSuccess(data);
    }
}