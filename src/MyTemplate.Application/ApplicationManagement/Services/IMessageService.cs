namespace MyTemplate.Application.ApplicationManagement.Services;

public interface IMessageService
{
    Task PublisAsync<T>(T message, CancellationToken cancellation = default);
}