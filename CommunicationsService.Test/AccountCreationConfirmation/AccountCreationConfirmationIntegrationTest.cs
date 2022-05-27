using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunicationsService.AccountCreationConfirmation;
using CommunicationsService.Test.Support;
using FluentAssertions;
using netDumbster.smtp;
using Xunit;
using static CommunicationsService.Test.Support.BffComponentTestApplication;

namespace CommunicationsService.Test.AccountCreationConfirmation;

public class AccountCreationConfirmationIntegrationTest
{
    [Fact]
    public async Task ProperEmailIsSentToExpectedRecipient()
    {
        var smtpServer = SimpleSmtpServer.Start(SmtpServerPort);
        var testApplicaton = new BffComponentTestApplication();
        var client = testApplicaton.CreateClient();
        await client.PostAsJsonAsync("/accountCreationConfirmation", new AccountCreationConfirmationRequest{Email = "tony@gmail.com", Name = "Johnny B Good"});

        smtpServer.ReceivedEmailCount.Should().Be(1);
        var sentEmail = smtpServer.ReceivedEmail[0];
        sentEmail.FromAddress.ToString().Should().Be(Config.EmailsUsername);
        sentEmail.ToAddresses[0].ToString().Should().Be("tony@gmail.com");
        sentEmail.Subject.Should().Be("Subscription Confirmation");
        sentEmail.MessageParts[0].BodyData.Should().Contain("Johnny B Good");
    }
}