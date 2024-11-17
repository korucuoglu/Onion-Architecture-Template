namespace MyTemplate.Application.AuthManagement.ValidateResetPasswordToken;

public class Command : CommandBase
{
    public required string Token { get; set; }
}