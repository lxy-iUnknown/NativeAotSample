using System.Diagnostics.CodeAnalysis;
using CommandDotNet;
using CommandDotNet.Builders;
using CommandDotNet.Logging;

namespace NativeAotSample;

internal static class Program
{
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors |
        DynamicallyAccessedMemberTypes.PublicProperties |
        DynamicallyAccessedMemberTypes.PublicMethods,
        typeof(RootCommand)
    )]
    private static int Main(string[] args)
    {
        LogProvider.IsDisabled = true;
        // NativeAOT compatible AppInfo resolver
        AppInfo.SetResolver(AppInformation.ResolveAppInfo);
        return new AppRunner<RootCommand>().Run(args);
    }
}