using Vintagestory.API.Common;

namespace FreeImmersiveTools.Config;

public static class ModConfigLoader
{
    public static ModConfig LoadOrDefault(ICoreAPI api)
    {
        try
        {
            var config = api.LoadModConfig<ModConfig>(ModConstants.ConfigFileName);
            if (config is not null)
            {
                return MigrateIfNeeded(api, config);
            }

            var defaults = ModConfig.Default;
            api.StoreModConfig(defaults, ModConstants.ConfigFileName);
            return defaults;
        }
        catch (System.Exception ex)
        {
            api.Logger.Warning($"[{ModConstants.ModId}] Failed to load config '{ModConstants.ConfigFileName}'. Falling back to defaults. {ex.Message}");
            return ModConfig.Default;
        }
    }

    private static ModConfig MigrateIfNeeded(ICoreAPI api, ModConfig config)
    {
        if (config.ConfigVersion == ModConfig.CurrentVersion)
        {
            return config;
        }

        if (config.ConfigVersion > ModConfig.CurrentVersion)
        {
            api.Logger.Warning($"[{ModConstants.ModId}] Config version {config.ConfigVersion} is newer than supported version {ModConfig.CurrentVersion}. Using loaded values as-is.");
            return config;
        }

        var migrated = new ModConfig
        {
            ConfigVersion = ModConfig.CurrentVersion,
            EnableSharedStartupLog = config.EnableSharedStartupLog,
            EnableServerStartupLog = config.EnableServerStartupLog,
            EnableClientStartupLog = config.EnableClientStartupLog
        };

        api.StoreModConfig(migrated, ModConstants.ConfigFileName);
        api.Logger.Notification($"[{ModConstants.ModId}] Migrated config from version {config.ConfigVersion} to {ModConfig.CurrentVersion}.");

        return migrated;
    }
}
