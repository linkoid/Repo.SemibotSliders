using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MenuLib;
using MenuLib.MonoBehaviors;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Pool;

namespace Linkoid.Repo.SemibotSliders;

[BepInPlugin("Linkoid.Repo.SemibotSliders", "Semibot Sliders", "0.5")]
[BepInDependency("nickklmao.menulib", "2.2")]
public class SemibotSliders : BaseUnityPlugin
{
    internal static SemibotSliders Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    BodySliderMenu bodySliderMenu = new BodySliderMenu();

    internal static ModConfigModel ConfigModel { get; private set; } = null!;

    private void Awake()
    {
        Instance = this;

        // Prevent the plugin from being deleted
        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        ConfigModel = new ModConfigModel(Config);
        Config.SettingChanged += OnConfigSettingChanged;

        UserConfigModel.InitGlobalUserConfig();

        Patch();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void Start()
    {
        //MenuAPI.AddElementToLobbyMenu(parent =>
        //{
        //    var button = MenuAPI.CreateREPOButton("Semibot Sliders", bodySliderMenu.Open, parent, new Vector3(443f, 13f, 0f));
        //    button.menuButton.colorClick = Color.white;
        //    button.menuButton.colorHover = new Color(1f, 0.9022f, 0f, 1f);
        //    button.menuButton.colorNormal = new Color(1f, 0.594f, 0f, 1f);
        //    button.menuButton.customColors = true;
        //});

        //MenuAPI.AddElementToEscapeMenu(parent =>
        //{
        //    var button = MenuAPI.CreateREPOButton("Semibot Sliders", bodySliderMenu.Open, parent, new Vector3(443f, 13f, 0f));
        //    button.menuButton.colorClick = Color.white;
        //    button.menuButton.colorHover = new Color(1f, 0.9022f, 0f, 1f);
        //    button.menuButton.colorNormal = new Color(1f, 0.594f, 0f, 1f);
        //    button.menuButton.customColors = true;
        //});

        MenuAPI.AddElementToColorMenu(parent =>
        {
            var menuPageColor = parent.GetComponent<MenuPageColor>();

            var button = MenuAPI.CreateREPOButton("Semibot Sliders", () => { menuPageColor.ConfirmButton(); bodySliderMenu.Open(); }, parent, new Vector3(495f, 45f, 0f));
            button.menuButton.colorClick = Color.white;
            button.menuButton.colorHover = new Color(1f, 0.9022f, 0f, 1f);
            button.menuButton.colorNormal = new Color(1f, 0.594f, 0f, 1f);
            button.menuButton.customColors = true;
        });
    }

    internal void Patch()
    {
        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll();
    }

    internal void Unpatch()
    {
        Harmony?.UnpatchSelf();
    }

    private void OnConfigSettingChanged(object sender, SettingChangedEventArgs e)
    {
        foreach (var playerAvatarSliders in Object.FindObjectsOfType<PlayerAvatarSliders>())
        {
            playerAvatarSliders.SetDirty();
        }
    }
}