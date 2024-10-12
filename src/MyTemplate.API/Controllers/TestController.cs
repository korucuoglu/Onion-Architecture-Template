using Common.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyTemplate.API.Controllers;

[Authorize]
[Route("api/test")]
public class TestController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] Application.TestManagement.GetAll.Query query)
      => Result(await Mediator.Send(query));
}