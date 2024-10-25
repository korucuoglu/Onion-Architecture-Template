namespace MyTemplate.Application.ApplicationManagement.Services;

public interface ITokenService
{
    public string CreateToken(int userId, DateTime expiresIn);
}