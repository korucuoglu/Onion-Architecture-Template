using Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using MyTemplate.Application.ApplicationManagement.Repositories.EF;
using MyTemplate.Domain.Entities;
using Newtonsoft.Json;

namespace MyTemplate.API.Attributes;

public class NotFoundFilterAttribute<TModel, TId> : IAsyncActionFilter where TModel : BaseEntity<TId>, new()
{
    private readonly IEFUnitOfWork _unitOfWork;

    public NotFoundFilterAttribute(IEFUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var idValue = context.RouteData?.Values["id"]?.ToString();

        if (string.IsNullOrWhiteSpace(idValue))
        {
            throw new CustomException("Id değeri bulunamadı");
        }

        var id = JsonConvert.DeserializeObject<TId>(idValue);
        
        await using var unitOfWork = _unitOfWork;

        var repository = unitOfWork.GetRepository<TModel, TId>();

        if (!await repository.AnyAsync(x => x.Equals(id)))
        {
            throw new CustomException("Böyle bir veri bulunamadı");
        }

        await next();
    }
}