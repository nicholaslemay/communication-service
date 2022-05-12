using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using netDumbster.smtp;
using Xunit;

namespace CommunicationsService.Test;

public class BFFComponentTestApplication : WebApplicationFactory<Program>
{
}

public class IntegrationTest
{
    [Fact]
    public async Task ItWorksInIntegration()
    {
        var smtpServer = SimpleSmtpServer.Start(9999);
        var testApplicaton = new BFFComponentTestApplication();
        var client = testApplicaton.CreateClient();
        await client.GetAsync("/");
        
        
        smtpServer.ReceivedEmailCount.Should().Be(1);
        var sentEmail = smtpServer.ReceivedEmail[0];
        sentEmail.FromAddress.ToString().Should().Be(Config.EmailsUsername);
        sentEmail.ToAddresses[0].ToString().Should().Be(Config.DestinationEmail);
        sentEmail.Subject.Should().Be("Subscription Confirmation");
        sentEmail.MessageParts[0].BodyData.Should().Contain("Johnny Primo");
    }
}