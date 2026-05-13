# Free Immersive Tools

Free Immersive Tools is a Vintage Story mod that aims to turn eligible tool-based crafting recipes into in-world interactions.

Officially, the F stands for "Free." Unofficially, its exact expansion may depend on how many crafting menus you had to click through that day.

The target behavior is: tool + in-world placeable ingredient = altered tool + recipe output. For example, using an axe on a placed log should produce the same result, ingredient consumption, and tool wear that the original recipe would have produced in a crafting grid.

If the tool does its job in-world and the result feels a little more forcefully immersive than before, that is very much the idea.

## Project status

This project is in active early development.

The repository currently contains the development scaffold, build and debug workflow, persisted configuration, config migration support, and optional ConfigLib integration for in-game editing of current settings. The core recipe adaptation and in-world execution system is planned work and is documented in [PRD.md](PRD.md).

## Goals

- Add immersive in-world execution for eligible tool + placeable-ingredient recipes.
- Preserve vanilla and modded recipe parity for inputs, outputs, and tool alteration.
- Stay compatible with other mods by discovering recipes late enough in the lifecycle.
- Keep optional integrations, such as ConfigLib, non-breaking when absent.

## Current implementation

- .NET 10 mod project and Cake-based packaging pipeline.
- VS Code tasks and launch profiles configured to use `bin/latest` as the local Vintage Story install.
- File-backed mod config with default generation and version migration.
- Optional ConfigLib asset integration for editing current startup logging settings in-game.
- Initial feature-oriented project structure under `ModSystems`, `Config`, `Behaviors`, and `Integration`.

## Requirements

- .NET 10 SDK
- Vintage Story game files available locally
- `VINTAGE_STORY` environment variable set for CLI, Rider, or Visual Studio workflows

This repository expects a local Vintage Story install to be available through `bin/latest`. In VS Code, build and launch configurations already set `VINTAGE_STORY` to `${workspaceFolder}/bin/latest` automatically.

## Setup

1. Place a Vintage Story install inside `bin/` and point `bin/latest` at the version you want to develop against.
2. For terminal, Rider, or Visual Studio workflows, export `VINTAGE_STORY` to that same folder.
3. Build and run the mod from your IDE or from the command line.

### Linux and macOS

```sh
export VINTAGE_STORY="$PWD/bin/latest"
```

### Windows PowerShell

```powershell
$env:VINTAGE_STORY = "$PWD/bin/latest"
```

## Development

### VS Code

Available tasks:

- `build`: Builds the mod project in Debug.
- `build (Cake)`: Builds the packaging project.
- `package`: Runs the Cake packaging pipeline.

Available debug profiles:

- `Launch Client (Debug)`
- `Launch Server`
- `CakeBuild`

These launch profiles already set `VINTAGE_STORY` to `${workspaceFolder}/bin/latest`.

### Rider and Visual Studio

Set `VINTAGE_STORY` before launching from the IDE.

Launch profiles in [FreeImmersiveTools/Properties/launchSettings.json](FreeImmersiveTools/Properties/launchSettings.json):

- `Client` and `Server` for Linux/macOS
- `Client (Windows)` and `Server (Windows)` for Windows

### Command line

Build the mod:

```sh
export VINTAGE_STORY="$PWD/bin/latest"
dotnet build -c Debug FreeImmersiveTools/FreeImmersiveTools.csproj
```

Build the Cake project:

```sh
export VINTAGE_STORY="$PWD/bin/latest"
dotnet build -c Debug CakeBuild/CakeBuild.csproj
```

Package the mod:

```sh
export VINTAGE_STORY="$PWD/bin/latest"
dotnet run --project CakeBuild/CakeBuild.csproj
```

## Configuration

The mod stores its config as `freeimmersivetools.json` in the Vintage Story mod config directory.

Current settings:

- `EnableSharedStartupLog`
- `EnableServerStartupLog`
- `EnableClientStartupLog`

The config file is generated automatically with defaults on first load. Older config versions are migrated forward when possible.

## ConfigLib support

ConfigLib support is optional.

If ConfigLib is installed and active, the current settings can be edited through its in-game UI using the integration asset at `assets/freeimmersivetools/config/configlib-patches.json`. If ConfigLib is not installed, the mod continues to work through its normal file-backed config.

## Packaging output

The Cake pipeline writes release artifacts to `Releases/` and produces a zip named like:

```text
freeimmersivetools_<version>.zip
```

The version comes from [FreeImmersiveTools/modinfo.json](FreeImmersiveTools/modinfo.json).

## Repository layout

```text
FreeImmersiveTools/   Main mod project
CakeBuild/            Packaging and JSON validation pipeline
bin/latest/           Local Vintage Story install used for development
PRD.md                Product requirements document
```

## References

- Vintage Story modding wiki: <https://wiki.vintagestory.at/Modding>
- Vintage Story API docs: <https://apidocs.vintagestory.at/>
- ConfigLib repository: <https://github.com/maltiez2/vsmod_configlib>
- ConfigLib JSON API: <https://github.com/maltiez2/vsmod_configlib/wiki/JSON-API>

## License

See [LICENSE](LICENSE).
