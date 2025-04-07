using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Linkoid.Repo.SemibotSliders;

internal class UserConfigModel
{
    public static UserConfigModel Instance { get; private set; } = null!;

    internal static void InitGlobalUserConfig()
    {
        var globalConfigPath = $"{Application.persistentDataPath}/REPOModData/Linkoid.Repo.SemibotSliders.User.cfg";
        var configFile = new ConfigFile(globalConfigPath, true, SemibotSliders.Instance.Info.Metadata);
        Instance = new UserConfigModel(configFile);
    }

    public readonly ConfigFile ConfigFile;

    public const string ScaleSettingsSection = "ScaleSettings";
    private readonly IReadOnlyDictionary<FieldInfo, ConfigEntryBase> ScaleSettingsEntries;

    private UserConfigModel(ConfigFile configFile)
    {
        ConfigFile = configFile;
        var scaleSettingsDictionary = new Dictionary<FieldInfo, ConfigEntryBase>();
        foreach (var field in typeof(ScaleSettings).GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            var configEntry = ConfigFile.Bind(ScaleSettingsSection, field.Name, (Vector3)field.GetValue(ScaleSettings.Default));
            scaleSettingsDictionary.Add(field, configEntry);
        }
        ScaleSettingsEntries = scaleSettingsDictionary;
    }

    public ScaleSettings GetScaleSettings()
    {
        object scaleSettings = new ScaleSettings();
        foreach ((var field, var config) in ScaleSettingsEntries)
        {
            field.SetValue(scaleSettings, config.BoxedValue);
        }
        return (ScaleSettings)scaleSettings;
    }

    public void SaveScaleSettings(ScaleSettings scaleSettings)
    {
        ConfigFile.SaveOnConfigSet = false;
        try
        {
            foreach ((var field, var configEntry) in ScaleSettingsEntries)
            {
                configEntry.BoxedValue = field.GetValue(scaleSettings);
            }
        }
        finally
        {
            ConfigFile.SaveOnConfigSet = true;
        }
        ConfigFile.Save();
    }
}
