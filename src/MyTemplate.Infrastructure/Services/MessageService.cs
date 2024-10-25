using MassTransit;
using MyTemplate.Application.ApplicationManagement.Services;

namespace MyTemplate.Infrastructure.Services;

public class MessageService : IMessageService
{
    private readonly IBus _bus;

    public MessageService(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublisAsync<T>(T message, CancellationToken cancellation = default)
    {
        await _bus.Publish(message, cancellation);
    }
}