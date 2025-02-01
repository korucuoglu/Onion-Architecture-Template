using Common.Interfaces;
using Common.Models;
using Common.Services;
using MassTransit;
using MyTemplate.WorkerService;
using MyTemplate.WorkerService.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailMessageConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("RabbitMQ:Hostname"), h =>
        {
            h.Username(builder.Configuration.GetValue<string>("RabbitMQ:Username"));
            h.Password(builder.Configuration.GetValue<string>("RabbitMQ:Password"));
        });

        cfg.ReceiveEndpoint("email_queue", e =>
        {
            e.ConfigureConsumer<EmailMessageConsumer>(context);
        });
    });
});



var host = builder.Build();
host.Run();