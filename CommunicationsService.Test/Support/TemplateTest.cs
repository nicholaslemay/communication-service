using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using RazorLight;

namespace CommunicationsService.Test.Support;

public abstract class TemplateTest
{
    private static readonly RazorLightEngine?   _templateEngine = new RazorLightEngineBuilder()
        .UseFileSystemProject("/Users/nick/RiderProjects/BDC/CommunicationsService/CommunicationsService/")
        .UseMemoryCachingProvider()
        .Build();

    protected IHtmlDocument RenderTemplate(string viewPath, object model)
    {
        var result = RenderViewAsync(viewPath, model).Result;
        return new HtmlParser().ParseDocument(result);
    }

    private Task<string> RenderViewAsync(string viewPath, object model)
    {
        return  _templateEngine.CompileRenderAsync(viewPath, model);
    }
}