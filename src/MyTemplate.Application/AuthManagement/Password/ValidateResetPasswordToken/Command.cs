namespace MyTemplate.Application.AuthManagement.Password.ValidateResetPasswordToken;

public class Command : CommandBase
{
    public required string Token { get; set; }
}