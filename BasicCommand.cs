using System.Diagnostics.CodeAnalysis;
using CommandDotNet;

namespace NativeAotSample;

public class PowerArgument : IArgumentModel
{
    [Positional("x")]
    public double X { get; set; }
    [Positional("y")]
    public double Y { get; set; }
}

public class MultiplyArgument : IArgumentModel
{
    [Positional("x")]
    public int X { get; set; }
    [Positional("y")]
    public int Y { get; set; }
}

public class BasicCommand
{
    [Command("add")]
    public void Add(
        [Positional("x")]
        int x,
        [Positional("y")]
        int y
    )
    {
        Console.WriteLine(x + y);
    }

    [Command("sub")]
    public void Subtract(
        [Positional("x")]
        int x,
        [Positional("y")]
        int y
    )
    {
        Console.WriteLine(x - y);
    }

    [Command("mul")]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicProperties,
        typeof(MultiplyArgument)
    )]
    public void Multiply(MultiplyArgument argument)
    {
        Console.WriteLine(argument.X * argument.Y);
    }
    
    [Command("pow")]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicProperties,
        typeof(PowerArgument)
    )]
    public void Power(PowerArgument argument)
    {
        Console.WriteLine(Math.Pow(argument.X, argument.Y));
    }
}