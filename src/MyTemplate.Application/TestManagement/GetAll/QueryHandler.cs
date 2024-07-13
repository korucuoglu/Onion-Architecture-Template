﻿using Common.Libraries.MediatR.Abstractions;
using Common.Libraries.MediatR.Models;

namespace MyTemplate.Application.TestManagement.GetAll;
public class QueryHandler : QueryHandlerBase<Query, int>
{
    protected override async Task<Result<int>> HandleAsync(Query request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}