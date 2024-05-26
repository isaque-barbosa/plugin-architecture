using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using PluginBase;

public class PluginMiddleware
{
    private readonly RequestDelegate _next;
    public PluginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var assemblyReference = await Process(context);

        if (!context.Response.HasStarted)
        {
            await _next(context);
        }

        while(assemblyReference.IsAlive)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }        
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> Process(HttpContext context)
    {
        var path = "\\source\\repos\\Plugin.Architecture\\HelloPlugin\\bin\\Debug\\net8.0\\HelloPlugin.dll";

        var loadContext = new AssemblyLoadContext(path, true);
        try
        {
            var assembly = loadContext.LoadFromAssemblyPath(path);
            var endpointType = assembly.GetType("HelloPlugin.HelloEndpoint");
            var pathInfo = endpointType?.GetCustomAttribute<PathAttribute>();

            if (pathInfo is not null &&
                pathInfo.Method.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase) &&
                pathInfo.Path.Equals(context.Request.Path, StringComparison.OrdinalIgnoreCase))
            {
                var endpoint = Activator.CreateInstance(endpointType) as IPluginEndpoint;
                await endpoint.Execute(context);
            }
        }
        finally
        {
            loadContext.Unload();
        }

        return new WeakReference(loadContext);
    }
}