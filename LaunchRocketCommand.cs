using System.Diagnostics.CodeAnalysis;
using CommandDotNet;

namespace NativeAotSample;

public class LaunchRocketArgument : IArgumentModel
{
    [Positional("planet")]
    public IEnumerable<string> Planets { get; set; }
    [Option('t', "turbo")]
    public bool? Turbo { get; set; }
    [Option('l', "log-level")]
    public LogLevel? Level { get; set; }
}

public class LaunchRocketCommand
{
    [Command("mul")]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicProperties,
        typeof(LaunchRocketArgument)
    )]
    [DefaultCommand]
    public void Execute(LaunchRocketArgument argument)
    {
        Console.WriteLine("Launching rocket ");
        Console.WriteLine($"Planets: {string.Join(", ", argument.Planets)}");
        Console.WriteLine($"Turbo: {argument.Turbo}");
        Console.WriteLine($"Log level: {argument.Level}");
    }
}