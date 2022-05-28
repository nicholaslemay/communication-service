using System.IO;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using RazorLight;

namespace CommunicationsService.Test.Support;

public abstract class TemplateTest
{
    private static readonly RazorLightEngine TemplateEngine = new RazorLightEngineBuilder()
        .UseFileSystemProject($"{Directory.GetCurrentDirectory()}/../../../../CommunicationsService/")
        .UseMemoryCachingProvider()
        .Build();

    protected static IHtmlDocument RenderTemplate(string viewPath, object model)
    {
        var result = RenderViewAsync(viewPath, model).Result;
        return new HtmlParser().ParseDocument(result);
    }

    private static Task<string> RenderViewAsync(string viewPath, object model) => 
        TemplateEngine.CompileRenderAsync(viewPath, model);
}