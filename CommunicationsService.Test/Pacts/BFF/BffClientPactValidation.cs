using CommunicationsService.Test.Pacts.Support;
using netDumbster.smtp;
using PactNet;
using Xunit;
using Xunit.Abstractions;

namespace CommunicationsService.Test.Pacts.BFF;

public class BffClientPactValidation
{
    private readonly ITestOutputHelper _output;

    public BffClientPactValidation(ITestOutputHelper output) => _output = output;

    [Fact]
    public void RespectContractsWithBffCLient()
    {
        var smtpServer = SimpleSmtpServer.Start(ContractTestApplication.SmtpServerPort);
        
        var app = new ContractTestApplication();
        app.Run();

        new PactVerifier(BffPactVerifierConfig.Build(_output))
            .ServiceProvider("CommunicationService", ContractTestApplication.BaseUrl)
            .HonoursPactWith("BFF")
            .PactUri($"{PactHelper.PactFolderLocation}/BFF/PactFiles/bff-communication_api.json")
            .Verify();
        
        smtpServer.Stop();
    }
}