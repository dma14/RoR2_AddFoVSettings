using System.IO;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using UnityEngine;

namespace AddFoVSettings
{
    [BepInDependency(RiskOfOptionsGuid, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class AddFoVSettingsMain : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Quizzik";
        public const string PluginName = "AddFoVSettings";
        public const string PluginVersion = "2.0.0";

        private const string RiskOfOptionsGuid = "com.rune580.riskofoptions";

        internal static ConfigEntry<float> BaseFoV { get; private set; }
        internal static ConfigEntry<bool> SprintingIncreasesFoV { get; private set; }

        private Harmony _harmony;

        public void Awake()
        {
            Log.Init(Logger);

            BaseFoV = Config.Bind(
                "Camera",
                "BaseCameraFoV",
                60f,
                new ConfigDescription(
                    "The base camera field of view (vanilla default is 60).",
                    new AcceptableValueRange<float>(50f, 120f)
                )
            );

            SprintingIncreasesFoV = Config.Bind(
                "Camera",
                "SprintingIncreasesFoV",
                false,
                "Enables the vanilla FoV increase while sprinting. Keep disabled to prevent sprint FoV zoom."
            );

            BaseFoV.SettingChanged += (_, _) => CameraFoVPatches.ApplyBaseFoVImmediate();

            _harmony = Harmony.CreateAndPatchAll(typeof(CameraFoVPatches), PluginGUID);

            if (Chainloader.PluginInfos.ContainsKey(RiskOfOptionsGuid))
            {
                RegisterRiskOfOptions();
            }

            Log.LogInfo($"{PluginName} {PluginVersion} initialized (BaseCameraFoV={BaseFoV.Value}, SprintingIncreasesFoV={SprintingIncreasesFoV.Value}).");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void RegisterRiskOfOptions()
        {
            ModSettingsManager.SetModDescription("Adjust base camera field of view and toggle sprint FoV scaling.");
            TryLoadModIcon();

            ModSettingsManager.AddOption(new StepSliderOption(
                BaseFoV,
                new StepSliderConfig
                {
                    name = "Base Camera FoV",
                    min = 50f,
                    max = 120f,
                    increment = 1f,
                    FormatString = "{0:0}"
                }
            ));

            ModSettingsManager.AddOption(new CheckBoxOption(
                SprintingIncreasesFoV,
                new CheckBoxConfig
                {
                    name = "Sprinting Increases FoV"
                }
            ));

            Log.LogInfo("Registered settings with Risk Of Options.");
        }

        private void TryLoadModIcon()
        {
            string pluginDir = Path.GetDirectoryName(Info.Location);
            if (string.IsNullOrEmpty(pluginDir))
            {
                return;
            }

            string iconPath = Path.Combine(pluginDir, "icon.png");
            if (!File.Exists(iconPath))
            {
                return;
            }

            byte[] iconBytes = File.ReadAllBytes(iconPath);
            Texture2D texture = new Texture2D(256, 256);
            if (ImageConversion.LoadImage(texture, iconBytes))
            {
                Sprite iconSprite = Sprite.Create(
                    texture,
                    new Rect(0f, 0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                ModSettingsManager.SetModIcon(iconSprite);
            }
        }
    }
}
