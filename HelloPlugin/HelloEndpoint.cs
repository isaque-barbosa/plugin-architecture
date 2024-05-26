using Microsoft.AspNetCore.Http;
using PluginBase;

namespace HelloPlugin;

[Path("get", "/plug/hello")]
public class HelloEndpoint : IPluginEndpoint
{
    public async Task Execute(HttpContext ctx)
    {
        await ctx.Response.WriteAsync("Hello World!");
    }
}
