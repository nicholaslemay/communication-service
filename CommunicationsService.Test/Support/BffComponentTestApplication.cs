using System.IO;
using System.Linq;
using System.Net.Mail;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CommunicationsService.Test.Support;

public class BffComponentTestApplication : WebApplicationFactory<Program>
{
    public const int SmtpServerPort = 9797;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => { config.AddJsonFile($"{Directory.GetCurrentDirectory()}/../../../Support/appsettings.contracttests.json"); });

        builder.ConfigureServices(services =>
        {
            var sender = services.Single(d => d.ServiceType == typeof(ISender));
            services.Remove(sender);

            services.TryAdd(ServiceDescriptor.Singleton<ISender>(_ => new SmtpSender(new SmtpClient
            {
                Host = "127.0.0.1",
                Port = SmtpServerPort,
            })));
        });
        
        return base.CreateHost(builder);
    }
}