namespace FreeImmersiveTools.Config;

public sealed class ModConfig
{
    public const int CurrentVersion = 1;

    public static ModConfig Default { get; } = new();

    public int ConfigVersion { get; init; } = CurrentVersion;
    public bool EnableSharedStartupLog { get; init; } = true;
    public bool EnableServerStartupLog { get; init; } = true;
    public bool EnableClientStartupLog { get; init; } = true;
}
