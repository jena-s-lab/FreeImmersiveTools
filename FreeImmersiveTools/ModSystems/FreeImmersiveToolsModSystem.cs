using FreeImmersiveTools.Behaviors;
using FreeImmersiveTools.Config;
using FreeImmersiveTools.Integration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace FreeImmersiveTools.ModSystems;

public class FreeImmersiveToolsModSystem : ModSystem
{
    private ModConfig config = ModConfig.Default;

    // Called on server and client
    public override void Start(ICoreAPI api)
    {
        config = ModConfigLoader.LoadOrDefault(api);
        IntegrationBootstrap.Initialize(api);
        StartupMessageBehavior.LogSharedStartup(Mod, config);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        StartupMessageBehavior.LogServerStartup(Mod, config);
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        StartupMessageBehavior.LogClientStartup(Mod, config);
    }
}
