# PRD: Free Immersive Tools

## 1. Product overview

### 1.1 Document title and version

- PRD: Free Immersive Tools
- Version: 0.1.0

### 1.2 Product summary

This tool adds immersive in-world crafting interactions to Vintage Story by adapting eligible recipes that use a tool and an ingredient that can exist as an in-world target. Instead of crafting only through menus, players can perform the action directly in the world, such as using an axe on a placed log or using the correct tool on a ground-stored item or material pile.

The system must preserve recipe behavior exactly: same ingredient consumption, same output, and same tool alteration (durability loss or other tool state changes) as the original crafting logic. The feature is compatibility-first and should behave like a new interaction layer, not a separate crafting economy.

The document defines scope, requirements, user experience, technical strategy, and delivery phases for an initial production-ready release focused on correctness, mod compatibility, and player immersion.

## 2. Goals

### 2.1 Business goals

- Achieve strong community adoption among Vintage Story players who value immersive gameplay.
- Deliver a stable, compatibility-friendly mod that can become a recommended baseline in modpacks.
- Enable long-term community support and optional donations through high quality and reliability.

### 2.2 User goals

- Craft eligible recipes in a natural in-world way using the correct tool on a placed ingredient, ground-stored item, or pile.
- Trust that outcomes match vanilla/modded crafting results exactly.
- Reduce reliance on UI crafting for immersive tool-based transformations.

### 2.3 Non-goals

- Creating brand-new tools.
- Creating brand-new recipes.
- Rebalancing recipe costs, outputs, or tool wear values.
- Replacing all crafting systems in the game.
- Building a custom crafting UI for this release.

## 3. User personas

### 3.1 Key user types

- Immersion-focused survival players
- Modpack players and server communities
- Modpack maintainers and server admins
- Compatibility-conscious mod users

### 3.2 Basic persona details

- **Immersion players**: Want actions like chopping, carving, and processing to happen in-world instead of only in menus.
- **Server players**: Need predictable multiplayer behavior and no desync when interactions occur.
- **Modpack maintainers**: Need broad compatibility with other content mods and low maintenance.
- **Power users**: Expect correctness and parity with recipe behavior from all loaded mods.

### 3.3 Role-based access

- **Single-player user**: Can use all in-world interactions based on loaded recipes and tools.
- **Multiplayer player**: Can perform interactions with server-authoritative validation and synced results.
- **Server admin/modpack maintainer**: Can enable/disable logging and validate compatibility with mod sets.

## 4. Functional requirements

- **Recipe eligibility discovery** (Priority: High)
  - Detect recipes that have exactly one ingredient that can be represented as an in-world target and one tool input (or equivalent eligible pattern).
  - Exclude recipes that do not meet eligibility rules.
  - Execute discovery after other mods have completed recipe registration.
- **In-world interaction trigger** (Priority: High)
  - On valid player action (e.g., right-click with tool on a placed block, ground-stored item, or pile), resolve candidate recipe mapping.
  - Validate player-held tool, target block/entity/ground-storage/pile state, and contextual constraints before applying.
- **Recipe parity execution** (Priority: High)
  - Apply the same input consumption and output generation as the original recipe.
  - Apply the same tool alteration as the original recipe.
  - Ensure no duplicate output or double-consumption occurs.
- **Result placement and feedback** (Priority: High)
  - Remove/alter in-world ingredient state as required by recipe, including decrementing pile or ground-stored quantities when applicable.
  - Spawn/drop outputs at the interaction position with expected pickup behavior.
  - Emit clear, minimal feedback on failed validation (optional debug logging).
- **Repeated pile workflow** (Priority: High)
  - Support eligible interactions on ground-stored items and piles, not only placed blocks.
  - When the target is a pile, execute exactly one recipe application per right-click.
  - Update the pile state after each successful interaction and stop cleanly when insufficient material remains.
- **Multiplayer authority and sync** (Priority: High)
  - Server validates and executes interactions.
  - Client receives synchronized world/tool/result updates.
- **Config support** (Priority: Medium)
  - Support config defaults and persisted config file.
  - Support optional ConfigLib integration for in-game settings edits.
- **Compatibility safeguards** (Priority: High)
  - Avoid hard dependency on ConfigLib.
  - Preserve behavior when ConfigLib is absent.
  - Avoid changing unrelated recipes or tool systems.
- **Diagnostics and observability** (Priority: Medium)
  - Controlled startup and migration logs.
  - Optional debug diagnostics for recipe adaptation counts and skip reasons.

## 5. User experience

### 5.1. Entry points & first-time user flow

- Player installs and enables the mod.
- On world load, the mod prepares eligible interaction mappings.
- Player uses a known valid tool on a valid placed ingredient, ground-stored item, or pile.
- Player sees immediate in-world transformation and output drop.
- Player confirms tool alteration and output parity with expected recipe behavior.

### 5.2. Core experience

- **Identify an eligible target**: Player finds an in-world ingredient target, such as a placed block, a ground-stored item, or a pile, that is known to support tool-based transformation.
  - The interaction should feel intuitive and consistent with physical world actions.
- **Use the correct tool**: Player right-clicks the target with the correct tool.
  - Invalid tool usage should fail safely with no state corruption.
- **Execute transformation**: The system validates, consumes, alters tool state, and creates output.
  - Results must be deterministic and match recipe rules exactly.
- **Continue gameplay naturally**: Player collects output and keeps progressing without opening crafting UI.
  - The flow should be fast, support repeated one-click interactions on piles, and reduce friction for repeat actions.

### 5.3. Advanced features & edge cases

- Late-loaded or modded recipes are included if registered before adaptation phase.
- Target block, ground-stored item, or pile changed/removed mid-action is safely handled.
- Tool breaks during action is handled consistently with recipe/tool rules.
- Pile or ground-stored quantity dropping below the required amount is safely handled.
- Inventory full or drop placement blocked falls back to safe drop behavior.
- Two players attempting the same target concurrently resolves correctly on server authority.
- Recipes with ambiguous mappings are skipped with diagnostic logs.
- Config version upgrades migrate safely to current schema.

### 5.4. UI/UX highlights

- No new mandatory UI for core crafting interactions.
- Uses natural world interactions as primary interface.
- Optional ConfigLib settings GUI support for toggles.
- Clear but minimal logs for troubleshooting.

## 6. Narrative

Alex is a survival-focused player who wants crafting to feel physically grounded instead of menu-driven. While building a workshop, Alex uses the correct tool on a placed ingredient and, later, repeatedly processes a material pile one right-click at a time to make a larger batch without opening a crafting screen. Each interaction produces the expected transformed output, applies tool wear exactly as expected, and keeps the flow of play fast, believable, and consistent with existing recipe logic.

## 7. Success metrics

### 7.1. User-centric metrics

- Percentage of eligible crafting actions performed in-world vs menu crafting.
- Player-reported immersion satisfaction (survey/feedback).
- Reduction in player friction complaints around tool-based crafting.
- Retention of users who install the mod over 30 days.

### 7.2. Business metrics

- Total installs/subscribers and active users.
- Inclusion in popular modpacks/servers.
- Positive ratings/recommendations and community mentions.
- Optional donation conversion from active users.

### 7.3. Technical metrics

- Recipe adaptation coverage rate for eligible recipes.
- Zero critical desync bugs in multiplayer validation tests.
- Error rate for failed interactions due to internal faults.
- Average interaction execution latency on server.
- Config migration success rate across version updates.

## 8. Technical considerations

### 8.1. Integration points

- Vintage Story recipe registry and lifecycle hooks.
- Block, ground-storage, and pile interaction handlers (client intent, server execution).
- Tool durability/state mutation pathways.
- Optional ConfigLib integration via assets/{domain}/config/configlib-patches.json.
- Mod config persistence via LoadModConfig/StoreModConfig.

### 8.2. Data storage & privacy

- Store mod settings in local/server mod config file.
- No personal user data collection required for core functionality.
- If telemetry is introduced later, keep opt-in and anonymized.

### 8.3. Scalability & performance

- Build recipe mappings once during startup/finalization phase.
- Avoid per-tick recipe scanning.
- Use indexed lookup by target ingredient + tool class/type.
- Support repeated pile interactions without rescanning recipes each click.
- Keep server-side validation lightweight and deterministic.
- Guard against heavy logging in production mode.

### 8.4. Potential challenges

- Identifying eligibility patterns across diverse modded recipe formats.
- Mapping placed blocks, ground-stored items, and piles to the same recipe intent without ambiguity.
- Ensuring exact parity for tool alteration and output behavior.
- Ensuring pile decrement and one-click-per-craft behavior stay deterministic under latency and multiplayer contention.
- Avoiding race conditions in multiplayer interactions.
- Maintaining compatibility across game/API updates and mod ecosystem changes.
- Handling recipes registered late or changed by other mods.

## 9. Milestones & sequencing

### 9.1. Project estimate

- Medium: 4-8 weeks for robust v0.1.0

### 9.2. Team size & composition

- Small Team: 1-2 total people
  - 1 mod developer (core systems, integration, testing)
  - Optional 1 QA/support collaborator (multiplayer and compatibility testing)

### 9.3. Suggested phases

- **Phase 1**: Foundations and recipe adaptation engine (1-2 weeks)
  - Key deliverables: Eligibility rules, startup adaptation timing, deterministic mapping cache.
- **Phase 2**: In-world execution pipeline (1-2 weeks)
  - Key deliverables: Interaction trigger, validation, parity execution, output drops.
- **Phase 3**: Multiplayer hardening and compatibility (1-2 weeks)
  - Key deliverables: Server-authoritative flow, concurrency handling, cross-mod tests.
- **Phase 4**: Config and optional ConfigLib integration (1 week)
  - Key deliverables: File-backed config, migration hook, ConfigLib patch assets and translations.
- **Phase 5**: Release readiness (1 week)
  - Key deliverables: Regression suite, performance checks, documentation, v0.1.0 release.

## 10. User stories

### 10.1. Perform an in-world recipe with the correct tool

- **ID**: US-001
- **Description**: As a player, I want to use the correct tool on a placed ingredient, ground-stored item, or pile to execute the corresponding recipe in-world so that crafting feels immersive.
- **Acceptance criteria**:
  - When I right-click a valid in-world target with the correct tool, the recipe executes.
  - The target ingredient is consumed/changed according to recipe behavior.
  - The expected recipe output appears in-world at the interaction location.

### 10.2. Preserve tool alteration parity

- **ID**: US-002
- **Description**: As a player, I want tool durability and tool-state changes to match standard crafting so that outcomes are fair and predictable.
- **Acceptance criteria**:
  - Tool durability/state after in-world crafting matches the equivalent recipe result.
  - If the tool would break in normal crafting, it breaks in the same conditions in-world.
  - No extra durability loss is applied.

### 10.3. Preserve recipe output parity

- **ID**: US-003
- **Description**: As a player, I want outputs from in-world crafting to exactly match recipe-defined outputs so that there is no balance difference.
- **Acceptance criteria**:
  - Output item IDs, stack sizes, and variants match recipe definitions.
  - No duplicate outputs occur from a single interaction.
  - No eligible output is missing from a successful interaction.

### 10.4. Handle invalid interactions safely

- **ID**: US-004
- **Description**: As a player, I want invalid tool-target actions to fail safely so that I do not lose items accidentally.
- **Acceptance criteria**:
  - If no eligible mapping exists, no ingredient is consumed.
  - If validation fails, no output is produced.
  - Failure does not alter unrelated tool or world state.

### 10.11. Repeatedly craft from piles and ground-stored targets

- **ID**: US-011
- **Description**: As a player, I want pile-compatible and ground-stored ingredients to support repeated in-world crafting so that I can process large quantities quickly without using the crafting UI.
- **Acceptance criteria**:
  - When an eligible ingredient is stored as a pile or ground-stored target, I can right-click it repeatedly with the correct tool to craft multiple units over time.
  - Each right-click performs exactly one recipe application.
  - The pile or ground-stored quantity updates after each successful craft.
  - The interaction stops safely when insufficient material remains for another craft.

### 10.5. Include modded recipes registered at startup completion

- **ID**: US-005
- **Description**: As a modpack player, I want recipes from other mods to be included when eligible so that the system works consistently across my mod set.
- **Acceptance criteria**:
  - Recipe adaptation runs after mod recipe registration is complete.
  - Eligible recipes from loaded mods are discoverable and usable in-world.
  - Ineligible or ambiguous recipes are skipped without crash.

### 10.6. Keep multiplayer interactions authoritative

- **ID**: US-006
- **Description**: As a multiplayer player, I want server-authoritative execution so that outcomes are synchronized and cheat-resistant.
- **Acceptance criteria**:
  - Server validates all interaction attempts before applying state changes.
  - All clients observe consistent world/tool/output results.
  - Concurrent interactions on the same target resolve deterministically.

### 10.7. Configure logging behavior through config

- **ID**: US-007
- **Description**: As a user, I want startup log toggles so that I can control verbosity.
- **Acceptance criteria**:
  - Shared, server, and client startup logging can be toggled via config.
  - Defaults preserve current behavior on first run.
  - Config persists between sessions.

### 10.8. Migrate older config versions automatically

- **ID**: US-008
- **Description**: As a returning user, I want old config files to migrate safely so that updates do not break my setup.
- **Acceptance criteria**:
  - Older config versions are upgraded to current schema on load.
  - Migrated config is persisted.
  - Invalid config falls back to defaults with a warning log.

### 10.9. Support optional ConfigLib in-game configuration UI

- **ID**: US-009
- **Description**: As a player, I want in-game settings editing when ConfigLib is installed so that I can configure behavior without manual file edits.
- **Acceptance criteria**:
  - When ConfigLib is installed, configured settings appear in ConfigLib UI.
  - When ConfigLib is absent, the mod still works with file/default config.
  - ConfigLib integration does not introduce a hard dependency in modinfo.

### 10.10. Ensure no recipe economy changes in v0.1.0

- **ID**: US-010
- **Description**: As a player and server admin, I want the mod to avoid adding recipes or tools so that the release remains focused and compatible.
- **Acceptance criteria**:
  - v0.1.0 introduces no new tools.
  - v0.1.0 introduces no new recipes.
  - Existing eligible recipes are only given an in-world execution path.
