using System;
using System.Reflection;
using Plugin.Architecture;
using PluginBase;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<PluginMiddleware>();

string[] pluginPaths = new string[]
{
    // Paths to plugins to load.
    @"\source\repos\Plugin.Architecture\HelloPlugin\bin\Debug\net8.0\HelloPlugin.dll"
};

IEnumerable<ICommand> commands = pluginPaths.SelectMany(pluginPath =>
{
    Assembly pluginAssembly = LoadPlugins.LoadPlugin(pluginPath);
    return LoadPlugins.CreateCommands(pluginAssembly);
}).ToList();

foreach(var command in commands)
{
    command.Execute();
}

app.UseRouting();
app.MapControllers();

app.Run();
