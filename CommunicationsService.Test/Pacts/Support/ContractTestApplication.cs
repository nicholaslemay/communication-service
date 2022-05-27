using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace CommunicationsService.Test.Pacts.Support;
public static class BffPactVerifierConfig
{
    public static PactVerifierConfig Build(ITestOutputHelper output) => new()
    {
        Outputters = new List<IOutput>
        {
            new XUnitOutput(output)
        }
    };
}

public class XUnitOutput : IOutput
{
    private readonly ITestOutputHelper _output;

    public XUnitOutput(ITestOutputHelper output)
    {
        _output = output;
    }

    public void WriteLine(string line)
    {
        _output.WriteLine(line);
    }

}

public static class PactHelper
{
    public static string PactFolderLocation => $"{Directory.GetCurrentDirectory()}/../../../Pacts";
}
public class ContractTestApplication : WebApplicationFactory<Program>
{
    public static string BaseUrl => "http://localhost:56566";
    public static int SmtpServerPort => 9999;
    
    private bool _disposed;
    private IHost? _host;

    public void Run() => EnsureServer();

    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _host!.Services!;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureKestrel(
            options => options.Listen(IPAddress.Any, 56566));
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile($"{Directory.GetCurrentDirectory()}/../../../Support/appsettings.contracttests.json");
        });
        
        builder.ConfigureServices(services =>
        {
            var sender = services.Single(d => d.ServiceType == typeof(ISender));
            services.Remove(sender);
            
            services.TryAdd(ServiceDescriptor.Singleton<ISender>(_ => new SmtpSender(new SmtpClient
            {
                Host = "127.0.0.1",
                Port = 9999,
            })));
        });
        
        // Create the host for TestServer now before we
        // modify the builder to use Kestrel instead.
        var testHost = builder.Build();

        // Modify the host builder to use Kestrel instead
        // of TestServer so we can listen on a real address.
        // Create and start the Kestrel server before the test server,
        // otherwise due to the way the deferred host builder works
        // for minimal hosting, the server will not get "initialized
        // enough" for the address it is listening on to be available.
        builder.ConfigureWebHost(p => p.UseKestrel());
        _host = builder.Build();
        _host.Start();
        
        //testHost.Start();
        return testHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_disposed) return;
        
        if (disposing) 
            _host?.Dispose();

        _disposed = true;
    }

    private void EnsureServer()
    {
        // This forces WebApplicationFactory to bootstrap the server
        using var _ = CreateDefaultClient();
    }
}