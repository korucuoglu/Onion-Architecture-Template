using Common.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace MyTemplate.API.Controllers;

[Authorize]
public class TestController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] Application.TestManagement.GetAll.Query query)
      => Result(await Mediator.Send(query));
}