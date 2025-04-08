using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MenuLib;
using MenuLib.MonoBehaviors;
using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using UnityEngine.UIElements;

namespace Linkoid.Repo.SemibotSliders;

internal class BodySliderMenu
{
    public REPOPopupPage? page;

    private FieldInfo[] fields;

    private ScaleSettings scaleSettings = ScaleSettings.Default;

    private MenuLib.MonoBehaviors.REPOAvatarPreview? avatarPreview;

    private Dictionary<FieldInfo, REPOVector3SliderModel> currentSliders;

    public BodySliderMenu()
    {
        fields = typeof(ScaleSettings).GetFields(BindingFlags.Instance | BindingFlags.Public);
    }

    public void Open()
    {
        scaleSettings = UserConfigModel.Instance.GetScaleSettings();

        page = MenuAPI.CreateREPOPopupPage("Semibot Sliders", REPOPopupPage.PresetSide.Left, shouldCachePage: false, pageDimmerVisibility: true);

        MenuAPI.CreateREPOButton("Confirm", Close, page.transform, new Vector2(66f, 18f));
        MenuAPI.CreateREPOButton("Revert", Revert, page.transform, new Vector2(260f, 18f));
        var resetButton = MenuAPI.CreateREPOButton("Reset to Defaults", Reset, page.transform);
        page.scrollView.popupPage.AddElementToScrollView(resetButton.rectTransform);

        currentSliders = new();
        foreach (FieldInfo field in fields)
        {
            var min = Vector3.one * SemibotSliders.ConfigModel.ScaleMinimum.Value;
            var max = Vector3.one * SemibotSliders.ConfigModel.ScaleMaximum.Value;
            var slider = new REPOVector3SliderModel(field.Name, "", value => OnValueChanged(field, value), page.scrollView,
                min, max, defaultValue: (Vector3)field.GetValue(scaleSettings));
            currentSliders.Add(field, slider);
        }

        avatarPreview = MenuAPI.CreateREPOAvatarPreview(page.transform, new Vector2(476f, 36f));

        page.OpenPage(openOnTop: false);
    }


    public void Revert()
    {
        scaleSettings = UserConfigModel.Instance.GetScaleSettings();
        ApplyScaleSettingsToSliders();
        ApplyScaleSettingsToMenuCharacter();
    }

    public void Reset()
    {
        scaleSettings = ScaleSettings.Default;
        ApplyScaleSettingsToSliders();
        ApplyScaleSettingsToMenuCharacter();
    }

    public void Close()
    {
        UserConfigModel.Instance.SaveScaleSettings(scaleSettings);

        if (PlayerController.instance?.playerAvatar?.TryGetComponent(out PlayerAvatarSlidersSync playerAvatarSlidersSync) ?? false)
        {
            playerAvatarSlidersSync.SetScaleSettings(scaleSettings);
        }

        page?.ClosePage(true);
    }

    private void OnValueChanged(FieldInfo field, Vector3 value)
    {
        //SemibotSliders.Logger.LogDebug("Updated sliders!");

        object newScaleSettings = scaleSettings;
        field.SetValue(newScaleSettings, value);
        scaleSettings = (ScaleSettings)newScaleSettings;

        ApplyScaleSettingsToMenuCharacter();
    }

    private void ApplyScaleSettingsToSliders()
    {
        foreach ((var field, var slider) in currentSliders)
        {
            slider.SetValue((Vector3)field.GetValue(scaleSettings), false);
        }
    }

    private void ApplyScaleSettingsToMenuCharacter()
    {
        var playerAvatarSliders = avatarPreview!.playerAvatarVisuals.GetComponent<PlayerAvatarSliders>();
        playerAvatarSliders.ScaleSettings = scaleSettings;
    }
}
