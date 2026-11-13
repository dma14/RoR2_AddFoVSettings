## 2.0.0
- Updated for modern *Risk of Rain 2* camera internals (`CameraTargetParams` & `CameraModePlayerBasic`) so standalone compatibility patches are no longer required.
- Eliminated the brief FoV dip/spike when starting or stopping a sprint.
- Migrated settings UI to **Risk Of Options** (`Settings → Mod Options → AddFoVSettings`) and standard BepInEx configuration (`Quizzik.AddFoVSettings.cfg`).
- Removed `R2API` and `HookGenPatcher` dependencies so the mod is strictly client-side.

## 1.0.0
- Initial release.
