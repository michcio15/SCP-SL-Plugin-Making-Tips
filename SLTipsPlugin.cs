using System;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace SLTipsPlugin;

public class SLTipsPlugin : Plugin
{
    public override string Name { get; } = "SLTips";

    public override string Author { get; } = "michcio";

    public override Version Version { get; } = new Version(1, 0, 0, 0);

    public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);

    public override void Enable()
    {
        Logger.Info("Hello world");
    }

    public override void Disable()
    {
        Logger.Info("Goodbye world");
    }
}