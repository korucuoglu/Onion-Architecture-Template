namespace MyTemplate.Application.AuthManagement.Password.ResetPasswordConfirm;

public class Command : CommandBase
{
    public required string Token { get; set; }
    public required string Password { get; set; }
    public required string RePassword { get; set; }
}