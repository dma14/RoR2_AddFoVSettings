# AddFoVSettings

Adds a **Base Camera FoV** slider (`50`–`120`) and a **Sprinting Increases FoV** toggle to *Risk of Rain 2* via **Risk Of Options** (under `Settings → Mod Options → AddFoVSettings`).

## Features
- **Base Camera FoV (`50`–`120`, default `60`):** Scales camera field of view natively across survivors and camera overrides (such as sniper/skill zooms).
- **Sprinting Increases FoV (default `Off`):** Disables the `1.3x` camera zoom while sprinting to prevent motion sickness, without sprint-transition jitter and while keeping sprint speed-line particles intact.

## Changelog

**2.0.0**
- Updated for modern *Risk of Rain 2* camera internals (`CameraTargetParams` & `CameraModePlayerBasic`) so standalone compatibility patches are no longer required.
- Eliminated the brief FoV dip/spike when starting or stopping a sprint.
- Migrated settings UI to **Risk Of Options** and standard BepInEx configuration (`Quizzik.AddFoVSettings.cfg`).
- Removed legacy `R2API` and `HookGenPatcher` dependencies.

**1.0.0**
- Initial release (based on [KosmosisDire/ChangeFOVSettings](https://github.com/KosmosisDire/ChangeFOVSettings)).
