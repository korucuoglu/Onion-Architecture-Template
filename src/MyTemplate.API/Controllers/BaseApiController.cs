using Common.Libraries.MediatR.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyTemplate.API.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    [NonAction]
    protected static IActionResult Result(Result response)
    {
        return new ObjectResult(response.StatusCode == 204 ? null : response)
        {
            StatusCode = response.StatusCode
        };
    }
}