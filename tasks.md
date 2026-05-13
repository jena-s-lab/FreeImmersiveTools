# Free Immersive Tools task breakdown

This file decomposes the PRD user stories into independently completable tasks sized to stay under 2-3 days each.

## Estimation legend

- `1` point: under 2 hours
- `2` points: 2-4 hours
- `3` points: 4-8 hours
- `5` points: 1-2 days
- `8` points: 2-3 days
- `13+` points: too large, must split
- `XS`: 1-2 points
- `S`: 2-3 points
- `M`: 5 points
- `L`: 8 points

## Current baseline already in place

| ID | Baseline item | Stories | Status |
| --- | --- | --- | --- |
| B-001 | File-backed config with default generation | US-007 | Done |
| B-002 | Config version migration hook | US-008 | Done |
| B-003 | Optional ConfigLib asset integration | US-009 | Done |

These stories still need verification and release hardening, but they are not greenfield work anymore.

## User story to task mapping

| User story | Delivery tasks |
| --- | --- |
| US-001 Perform an in-world recipe with the correct tool | T-001, T-002, T-003, T-004, T-008, T-009 |
| US-002 Preserve tool alteration parity | T-005 |
| US-003 Preserve recipe output parity | T-006 |
| US-004 Handle invalid interactions safely | T-007 |
| US-005 Include modded recipes registered at startup completion | T-001, T-002, T-003, T-010 |
| US-006 Keep multiplayer interactions authoritative | T-011 |
| US-007 Configure logging behavior through config | B-001, T-012 |
| US-008 Migrate older config versions automatically | B-002, T-012 |
| US-009 Support optional ConfigLib in-game configuration UI | B-003, T-012 |
| US-010 Ensure no recipe economy changes in v0.1.0 | T-002, T-010, T-013 |
| US-011 Repeatedly craft from piles and ground-stored targets | T-008, T-009 |

## Dependency graph

```text
T-001 -> T-002 -> T-003 -> T-004 -> T-005 -> T-008 ->
                                      \-> T-006 -> T-009 -> T-011 -> T-013
                                      \-> T-007 /
T-003 -> T-010 ----------------------/
T-012 -------------------------------/
```

Critical path:

`T-001 -> T-002 -> T-003 -> T-004 -> T-005/T-006 -> T-009 -> T-011 -> T-013`

Parallel windows:

- After `T-004`: `T-005`, `T-006`, and `T-007`
- After `T-003`: `T-010` can run in parallel with `T-004`
- After `T-005` and `T-006`: `T-008` and `T-009`
- `T-012` can run in parallel with implementation work because it validates the existing config/configlib baseline

## Backlog

| ID | Task | Stories | Objective | Owned files/dirs | Estimate | Blocked by | Done when |
| --- | --- | --- | --- | --- | --- | --- | --- |
| T-001 | Spike recipe and target APIs | US-001, US-005, US-011 | Verify the real Vintage Story APIs and lifecycle points for recipe discovery, target types, and tool parity. | `PRD.md`, `tasks.md`, research notes only | 5 points, M, 1 day | - | Candidate APIs are documented and no open question blocks T-002. |
| T-002 | Define eligibility and target abstraction | US-001, US-004, US-005, US-010 | Define which recipes qualify and create one abstraction for placed blocks, ground-stored items, and piles. | `FreeImmersiveTools/Integration/`, `FreeImmersiveTools/Config/` | 3 points, S, 0.5-1 day | T-001 | Eligibility rules are explicit, ambiguous recipes are skippable, and target matching does not branch by target type. |
| T-003 | Build post-registration recipe discovery cache | US-001, US-005 | Discover eligible recipes after all mods register and cache them once. | `FreeImmersiveTools/Integration/`, `FreeImmersiveTools/ModSystems/` | 5 points, M, 1-2 days | T-002 | Discovery runs at the correct lifecycle point, caches once, and records skip reasons. |
| T-004 | Deliver placed-block walking skeleton | US-001 | Ship one end-to-end placed-block interaction path to prove the architecture. | `FreeImmersiveTools/ModSystems/`, `FreeImmersiveTools/Behaviors/`, `FreeImmersiveTools/Integration/` | 5 points, M, 1-2 days | T-003 | One known recipe works in-world, server-authoritatively, with no crafting UI. |
| T-005 | Implement tool alteration parity executor | US-002 | Match durability and tool-state changes to the original crafting recipe. | `FreeImmersiveTools/Behaviors/`, `FreeImmersiveTools/Integration/` | 3 points, S, 0.5-1 day | T-004 | Tool wear and breakage exactly match recipe expectations. |
| T-006 | Implement output parity and safe drop placement | US-003 | Match recipe outputs and place them safely in-world. | `FreeImmersiveTools/Behaviors/`, `FreeImmersiveTools/Integration/` | 3 points, S, 0.5-1 day | T-004 | Output IDs, variants, and counts match recipes with no duplicates. |
| T-007 | Add invalid interaction safeguards | US-004 | Fail invalid tool-target actions safely. | `FreeImmersiveTools/Behaviors/`, `FreeImmersiveTools/Integration/` | 3 points, S, 0.5-1 day | T-004 | Invalid interactions do not consume ingredients, produce output, or mutate unrelated state. |
| T-008 | Add ground-storage target support | US-001, US-011 | Extend execution to eligible ground-stored item targets. | `FreeImmersiveTools/Integration/`, `FreeImmersiveTools/Behaviors/` | 5 points, M, 1-2 days | T-005, T-006 | Ground-stored targets can be processed correctly and fail safely when insufficient. |
| T-009 | Add pile support with one-craft-per-click | US-011 | Support repeated pile crafting with exactly one recipe application per right-click. | `FreeImmersiveTools/Integration/`, `FreeImmersiveTools/Behaviors/`, `FreeImmersiveTools/ModSystems/` | 8 points, L, 2-3 days | T-005, T-006 | Each click performs one craft, pile counts decrement correctly, and crafting stops safely on insufficient material. |
| T-010 | Expand coverage to modded recipes and ambiguity rules | US-005, US-010 | Generalize from the walking skeleton to eligible modded recipes without adding new recipes/tools. | `FreeImmersiveTools/Integration/`, `FreeImmersiveTools/Behaviors/` | 5 points, M, 1-2 days | T-003 | Eligible modded recipes are included, ambiguous ones are skipped, and recipe economy remains unchanged. |
| T-011 | Harden multiplayer authority and contention | US-006 | Make interaction results deterministic under multiplayer contention and latency. | `FreeImmersiveTools/ModSystems/`, `FreeImmersiveTools/Integration/`, `FreeImmersiveTools/Behaviors/` | 5 points, M, 1-2 days | T-008, T-009, T-010 | No duplicate outputs under contention, client state matches server state, and pile decrement stays deterministic. |
| T-012 | Verify config, migration, and ConfigLib UX | US-007, US-008, US-009 | Validate the existing config baseline and optional ConfigLib UX. | `FreeImmersiveTools/Config/`, `FreeImmersiveTools/assets/freeimmersivetools/config/`, `FreeImmersiveTools/assets/freeimmersivetools/lang/` | 3 points, S, 0.5-1 day | - | Defaults persist, migration works, and ConfigLib stays optional and non-breaking. |
| T-013 | Run regression matrix and release hardening | US-001 through US-011 | Validate all supported targets, parity rules, multiplayer behavior, and packaging for v0.1.0. | `README.md`, `PRD.md`, `tasks.md`, `FreeImmersiveTools/`, `CakeBuild/` | 5 points, M, 1-2 days | T-011, T-012 | Regression covers blocks, ground storage, piles, parity, discovery timing, multiplayer, and packaging. |

## Coordination notes

- Lane A: lifecycle, eligibility, and discovery cache
- Lane B: parity executors and invalid-safety guards
- Lane C: target adapters for ground storage and piles
- Lane D: compatibility expansion and ambiguity handling
- Lane E: multiplayer hardening
- Lane F: config verification, regression, docs, and release

Rebalancing guidance:

- If Lane C is blocked, move effort into T-010 or T-012 instead of adding scope.
- If T-001 finds high uncertainty in target APIs, keep T-002 narrow and schedule a second bounded spike rather than inflating downstream tasks.
- Do not merge T-008 and T-009 unless the code paths prove identical. Pile behavior has enough special-case risk to justify its own task.
