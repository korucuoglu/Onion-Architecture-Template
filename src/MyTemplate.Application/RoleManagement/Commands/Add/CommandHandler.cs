using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Extensions;

namespace MyTemplate.Application.RoleManagement.Commands.Add;

public class CommandHandler: CommandHandlerBase<Command>
{
    
    private readonly RoleManager<ApplicationRole> _roleManager;
    
    public CommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }
    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var isExists = await _roleManager.RoleExistsAsync(request.Name);

        if (isExists)
        {
            return Result.WithFailure(Error.WithMessage("İlgili rol zaten mevcut"));
        }

        var result = await _roleManager.CreateAsync(new ApplicationRole()
        {
            Name = request.Name
        });

        if (!result.Succeeded)
        {
            return Result.WithFailure(Error.WithMessage(result.GetErrorMessage("Rol kaydedilemedi")));
        }

        return Result.WithSuccess();
    }
}