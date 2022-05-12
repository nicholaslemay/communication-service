using System.Threading.Tasks;
using CommunicationsService.Test.Support;
using FluentAssertions;
using Xunit;

namespace CommunicationsService.Test.Templates;

public class MyTemplateTest : TemplateTest
{
    [Fact]
    public async Task RendersSalutation()
    {
        var model = new TestViewModel{Name = "Dude!"};
        var html =  RenderTemplate("Templates/MyTemplate.cshtml", model);
        html.GetElementById("salutation").TextContent.Should().Contain("Hello Dude");
    }
}