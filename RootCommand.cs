using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CommandDotNet;

namespace NativeAotSample;

public class RootCommand
{
    static RootCommand()
    {
        KeepInstantiation<int>();
        KeepInstantiation<double>();
    }
    
    // Keep some generic instantiations.
    // See https://github.com/dotnet/runtime/issues/81204
    private static void KeepInstantiation<T>()
    {
        // Simple no-op method (will be optimized in JIT mode and AOT mode)
        var value = Constant();
        if ((value ^ value) == 0) return;
        // Method body
        Type.GetType("CommandDotNet.Extensions.TypeExtensions, CommandDotNet")!
            .GetMethod(
                "GetDefaultValue",
                BindingFlags.NonPublic | BindingFlags.Static,
                Type.DefaultBinder,
                Type.EmptyTypes,
                [])!
            .MakeGenericMethod(typeof(T));
        return;

        static int Constant() => 0;
    }

    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicMethods,
        typeof(BasicCommand)
    )]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicMethods,
        typeof(LaunchRocketCommand)
    )]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicMethods,
        typeof(GreetingCommand)
    )]
    public RootCommand()
    {
        
    }
    
    [Subcommand(RenameAs = "basic")] 
    public BasicCommand BasicCommand { get; set; }
    
    [Subcommand(RenameAs = "launch")] 
    public LaunchRocketCommand LaunchRocketCommand { get; set; }
    
    [Subcommand(RenameAs = "greet")] 
    public GreetingCommand GreetingCommand { get; set; }
}