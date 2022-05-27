using System.Threading.Tasks;
using CommunicationsService.AccountCreationConfirmation;
using CommunicationsService.Test.Support;
using FluentAssertions;
using Xunit;

namespace CommunicationsService.Test.AccountCreationConfirmation;

public class AccountCreationConfirmationEmailTemplateTest : TemplateTest
{
    [Fact]
    public async Task RendersSalutation()
    {
        var model = new AccountCreationConfirmationEmailViewModel{Name = "Dora"};
        var rendered =  RenderTemplate("AccountCreationConfirmation/AccountCreationConfirmationEmailTemplate.cshtml", model);
        rendered.GetElementById("salutation").TextContent.Should().Contain("Dora");
    }
}