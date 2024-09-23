using Common.Controller;
using Microsoft.AspNetCore.Mvc;

namespace MyTemplate.API.Controllers;

[Route("api/[controller]")]
public class TestController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Application.TestManagement.GetAll.Query query)
      => Result(await Mediator.Send(query));
}