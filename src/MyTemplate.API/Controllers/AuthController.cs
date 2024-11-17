using Microsoft.AspNetCore.Mvc;

namespace MyTemplate.API.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{

    [HttpGet("validate/mail/{Token}")]
    public async Task<IActionResult> ValidateMailAsync([FromRoute] Application.AuthManagement.ValidateMail.Command command)
     => Result(await Mediator.Send(command));
    
    [HttpGet("validate/token/{Token}")]
    public async Task<IActionResult> ValidateTokenAsync([FromRoute] Application.AuthManagement.ValidateToken.Command command)
        => Result(await Mediator.Send(command));

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Application.AuthManagement.Register.Command command)
      => Result(await Mediator.Send(command));

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] Application.AuthManagement.Login.Command command)
     => Result(await Mediator.Send(command));

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] Application.AuthManagement.Update.Command command)
   => Result(await Mediator.Send(command));

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] Application.AuthManagement.ChangePassword.Command command)
  => Result(await Mediator.Send(command));
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] Application.AuthManagement.ResetPassword.Command command)
        => Result(await Mediator.Send(command));
    
    [HttpPost("reset-password/confirm")]
    public async Task<IActionResult> ResetPasswordConfirmAsync([FromBody] Application.AuthManagement.ResetPasswordConfirm.Command command)
        => Result(await Mediator.Send(command));
    
  
}