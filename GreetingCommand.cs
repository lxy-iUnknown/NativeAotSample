using CommandDotNet;

namespace NativeAotSample;

public class GreetingCommand
{
    [DefaultCommand]
    public void Execute([Positional("name")] string name)
    {
        Console.WriteLine($"Hello, {name}");
    }
}