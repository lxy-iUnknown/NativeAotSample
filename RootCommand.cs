using System.Diagnostics.CodeAnalysis;
using CommandDotNet;

namespace NativeAotSample;

public class RootCommand
{
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicProperties |
        DynamicallyAccessedMemberTypes.PublicMethods,
        typeof(BasicCommand)
    )]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicProperties |
        DynamicallyAccessedMemberTypes.PublicMethods,
        typeof(LaunchRocketCommand)
    )]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicProperties |
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