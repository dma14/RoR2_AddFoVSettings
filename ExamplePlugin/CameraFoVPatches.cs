using HarmonyLib;
using RoR2;
using RoR2.CameraModes;

namespace AddFoVSettings
{
    [HarmonyPatch]
    internal static class CameraFoVPatches
    {
        private const float VanillaDefaultFoV = 60f;

        internal static void ApplyBaseFoVImmediate()
        {
            float targetFoV = AddFoVSettingsMain.BaseFoV.Value;

            foreach (CameraRigController cameraRig in CameraRigController.readOnlyInstancesList)
            {
                if (!cameraRig)
                {
                    continue;
                }

                cameraRig.baseFov = targetFoV;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CameraRigController), nameof(CameraRigController.OnEnable))]
        private static void CameraRigController_OnEnable_Postfix(CameraRigController __instance)
        {
            __instance.baseFov = AddFoVSettingsMain.BaseFoV.Value;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CameraTargetParams), nameof(CameraTargetParams.CalcParams))]
        private static void CameraTargetParams_CalcParams_Postfix(ref CharacterCameraParamsData dest)
        {
            float scale = AddFoVSettingsMain.BaseFoV.Value / VanillaDefaultFoV;
            dest.fov.value *= scale;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CameraModeBase), nameof(CameraModeBase.Update))]
        private static void CameraModeBase_Update_Prefix(
            CameraModeBase __instance,
            ref CameraModeBase.CameraModeContext context,
            out bool __state)
        {
            context.cameraInfo.baseFov = AddFoVSettingsMain.BaseFoV.Value;
            __state = context.targetInfo.isSprinting;

            if (!AddFoVSettingsMain.SprintingIncreasesFoV.Value && __instance is CameraModePlayerBasic)
            {
                context.targetInfo.isSprinting = false;

                if (context.cameraInfo.cameraRigController != null &&
                    __instance.camToRawInstanceData != null &&
                    __instance.camToRawInstanceData.TryGetValue(context.cameraInfo.cameraRigController, out object raw) &&
                    raw is CameraModePlayerBasic.InstanceData instanceData)
                {
                    instanceData.sprintFovMultiplier = 1f;
                    instanceData.sprintFovDampVelocity = 0f;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CameraModeBase), nameof(CameraModeBase.Update))]
        private static void CameraModeBase_Update_Postfix(
            CameraModeBase __instance,
            ref CameraModeBase.CameraModeContext context,
            ref CameraModeBase.UpdateResult result,
            bool __state)
        {
            context.targetInfo.isSprinting = __state;

            if (!AddFoVSettingsMain.SprintingIncreasesFoV.Value && __state && __instance is CameraModePlayerBasic)
            {
                result.showSprintParticles = true;
            }
        }
    }
}
