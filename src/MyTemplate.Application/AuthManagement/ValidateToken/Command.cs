namespace MyTemplate.Application.AuthManagement.ValidateToken;

public class Command : CommandBase
{
    public required string Token { get; set; }
}