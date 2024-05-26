using Microsoft.AspNetCore.Http;

namespace PluginBase;

public interface IPluginEndpoint
{
    Task Execute(HttpContext ctx);
}

public class PathAttribute : Attribute
{
    public PathAttribute(string method, string path)
    {
        Method = method;
        Path = path;
    }

    public string Method { get; }
    public string Path { get; }
}

public interface ICommand
{
    string Name { get; }
    string Description { get; }

    int Execute();
}
