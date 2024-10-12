using Common.Controller;
using Microsoft.AspNetCore.Mvc;

namespace MyTemplate.API.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Application.UserManagement.Register.Command command)
      => Result(await Mediator.Send(command));

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] Application.UserManagement.Login.Command command)
     => Result(await Mediator.Send(command));
}