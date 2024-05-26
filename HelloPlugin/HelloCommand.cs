using PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
namespace HelloPlugin;
public class HelloCommand : ICommand
{
    public string Name { get => "hello"; }
    public string Description { get => "Displays hello message."; }

    public int Execute()
    {
        Console.WriteLine("Hello !!!");
        return 0;
    }
}
