using BepInEx;
using RoR2;

namespace AddFoVSettings
{
    //This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(R2API.R2API.PluginGUID)]

    //This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class AddFoVSettingsMain : BaseUnityPlugin
    {
        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Quizzik";
        public const string PluginName = "AddFoVSettings";
        public const string PluginVersion = "1.0.0";

        // Custom stuff
        CameraRigController currentCamera;
        float sprintFoVModifier = 1.0F / 1.3F;
        MenuSlider FoVSlider;
        MenuCheckbox sprintFoVEffectsCheckbox;

        //The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            //Init our logging class so that we can properly log for debugging
            Log.Init(Logger);

            FoVSlider = new MenuSlider(60, 120, 50, true, "Base Camera FOV", "The base camera FOV to use.", SubPanel.Gameplay);
            FoVSlider.OnSliderChanged += (newValue) => { if (currentCamera) currentCamera.baseFov = newValue; };
            sprintFoVEffectsCheckbox = new MenuCheckbox(false, "Sprinting Increases FOV", "Enables an increase in FOV while sprinting.", SubPanel.Gameplay);

            On.RoR2.CameraRigController.OnEnable += hook_OnCameraEnable;

            // This line of log will appear in the bepinex console when the Awake method is done.
            Log.LogInfo(nameof(Awake) + " done.");
        }
        void hook_OnCameraEnable(On.RoR2.CameraRigController.orig_OnEnable orig, CameraRigController self)
        {
            orig(self);
            currentCamera = self;
            currentCamera.baseFov = FoVSlider.GetValue();
        }

        //The Update() method is run on every frame of the game.
        private void Update()
        {
            if (RoR2.Run.instance && !sprintFoVEffectsCheckbox.GetValue())
            {
                if (currentCamera.cameraModeContext.targetInfo.isSprinting)
                {
                    currentCamera.baseFov = FoVSlider.GetValue() * sprintFoVModifier;
                }
                else
                {
                    currentCamera.baseFov = FoVSlider.GetValue();
                }
            }

        }
    }
}
