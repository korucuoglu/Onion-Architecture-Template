﻿namespace MyTemplate.Application.TestManagement.GetAll;

public class QueryHandler : QueryHandlerBase<Query, int>
{
    protected override async Task<Result<int>> HandleAsync(Query request, CancellationToken cancellationToken)
    {
        var data = await Task.FromResult(55);
        
        return Result<int>.WithSuccess(data);
    }
}