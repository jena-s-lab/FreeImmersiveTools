using Vintagestory.API.Common;

namespace FreeImmersiveTools.Integration;

public static class IntegrationBootstrap
{
    public static void Initialize(ICoreAPI api)
    {
        // First refactor slice keeps runtime behavior unchanged.
        // This is the extension point for future cross-mod integration wiring.
    }
}
