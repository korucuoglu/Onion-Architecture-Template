using MassTransit;
using MyTemplate.WorkerService;
using MyTemplate.WorkerService.Consumers;
using MyTemplate.WorkerService.Models;
using MyTemplate.WorkerService.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));

builder.Services.AddTransient<MailService>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailMessageConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost"); // RabbitMQ sunucusunun adresi
        cfg.ReceiveEndpoint("email_queue", e =>
        {
            e.ConfigureConsumer<EmailMessageConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();