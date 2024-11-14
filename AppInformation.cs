using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using CommandDotNet.Builders;

namespace NativeAotSample;

internal static class AppInformation
{
    // See https://github.com/icsharpcode/ILSpy/blob/master/ICSharpCode.Decompiler/SingleFileBundle.cs
    // See https://github.com/dotnet/runtime/blob/master/src/installer/managed/Microsoft.NET.HostModel/AppHost/HostWriter.cs
    private static ReadOnlySpan<byte> SelfContainedSignature
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get =>
        [
            0x8b, 0x12, 0x02, 0xb9, 0x6a, 0x61, 0x20, 0x38,
            0x72, 0x7b, 0x93, 0x02, 0x14, 0xd7, 0xa0, 0x32,
            0x13, 0xf5, 0xb9, 0xe6, 0xef, 0xae, 0x33, 0x18,
            0xee, 0x3b, 0x2d, 0xce, 0x24, 0xb3, 0x6a, 0xae
        ];
    }

    private static unsafe bool IsSelfContained(string processPath)
    {
        try
        {
            using var mappedProcessPath = MemoryMappedFile.CreateFromFile(
                processPath,
                FileMode.Open,
                null,
                0,
                MemoryMappedFileAccess.Read
            );
            using var view = mappedProcessPath.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
            var handle = view.SafeMemoryMappedViewHandle;
            var size = handle.ByteLength;
            byte* data = null;
            handle.AcquirePointer(ref data);
            if (data == null) return false;
            try
            {
                var index = new ReadOnlySpan<byte>(data, checked((int)size))
                    .IndexOf(SelfContainedSignature);
                if (index < 0) return false;

                var offset = *(long*)(data + index - sizeof(long));
                return offset > 0 && offset < (long)size;
            }
            finally
            {
                handle.ReleasePointer();
            }
        }
        catch
        {
            return false;
        }
    }

    internal static AppInfo ResolveAppInfo()
    {
        var mainModule = Process.GetCurrentProcess().MainModule;
        var filePath = Environment.ProcessPath;
        var isRunViaDotnet =
            mainModule != null &&
            Path.GetFileName(mainModule.FileName).StartsWith("dotnet");
        var entryAssembly = typeof(Program).Assembly;
        var version = entryAssembly.GetName().Version?.ToString() ?? "0.0.0.0";
        if (filePath == null)
        {
            return new AppInfo(
                false,
                false,
                isRunViaDotnet,
                entryAssembly,
                "<Unknown>",
                "<Unknown>",
                version
            );
        }

        return new AppInfo(
            filePath.EndsWith("exe"),
            IsSelfContained(filePath),
            isRunViaDotnet,
            entryAssembly,
            filePath,
            Path.GetFileName(filePath),
            version
        );
    }
}