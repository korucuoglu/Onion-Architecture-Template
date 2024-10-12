namespace MyTemplate.Application.TestManagement.GetAll;

public class QueryHandler : QueryHandlerBase<Query, int>
{
    protected override async Task<Result<int>> HandleAsync(Query request, CancellationToken cancellationToken)
    {
        return Result<int>.WithSuccess(55);
    }
}