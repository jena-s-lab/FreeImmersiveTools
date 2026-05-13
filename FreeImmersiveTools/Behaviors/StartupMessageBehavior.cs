using FreeImmersiveTools.Config;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace FreeImmersiveTools.Behaviors;

public static class StartupMessageBehavior
{
    public static void LogSharedStartup(Mod mod, ModConfig config)
    {
        if (!config.EnableSharedStartupLog)
        {
            return;
        }

        mod.Logger.Notification("Hello from Free Immersive Tools: " + Lang.Get(ModConstants.HelloTranslationKey));
    }

    public static void LogServerStartup(Mod mod, ModConfig config)
    {
        if (!config.EnableServerStartupLog)
        {
            return;
        }

        mod.Logger.Notification("Hello from Free Immersive Tools server side");
    }

    public static void LogClientStartup(Mod mod, ModConfig config)
    {
        if (!config.EnableClientStartupLog)
        {
            return;
        }

        mod.Logger.Notification("Hello from Free Immersive Tools client side");
    }
}
