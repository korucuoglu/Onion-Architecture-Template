using Common.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace MyTemplate.API.Controllers;

[Route("api/[controller]")]
public class RoleController: BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromRoute] Application.RoleManagement.Queries.GetAll.Query command)
        => Result(await Mediator.Send(command));
    
    [HttpPost]
    public async Task<IActionResult> InsertAsync([FromBody] Application.RoleManagement.Commands.Add.Command command)
        => Result(await Mediator.Send(command));
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] Application.RoleManagement.Commands.UpdateById.Command command)
        => Result(await Mediator.Send(command));
    
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Application.RoleManagement.Commands.DeleteById.Command command)
        => Result(await Mediator.Send(command));
}