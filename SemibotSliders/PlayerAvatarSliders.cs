using HarmonyLib;
using UnityEngine;

namespace Linkoid.Repo.SemibotSliders;

[HarmonyPatch]
internal class PlayerAvatarSliders : MonoBehaviour
{
    private PlayerAvatarVisuals playerAvatarVisuals = null!;

    Transform? legLBot;
    Transform? legLTop;
    Transform? legRBot;
    Transform? legRTop;
    Transform? bodyBot;
    Transform? bodyBotScale;
    Transform? bodyTop;
    Transform? bodyTopScale;
    Transform? armL;
    Transform? armR;
    Transform? headBot;
    Transform? headTop;
    Transform? eyeL;
    Transform? eyeR;

    public ScaleSettings ScaleSettings
    {
        get => _scaleSettings;
        set
        {
            _scaleSettings = value;
            isDirty = true;
        }
    }
    private ScaleSettings _scaleSettings = ScaleSettings.Default;
    private bool isDirty = true;

    private ScaleSettings appliedScaleSettings = ScaleSettings.Default;
    

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerAvatarVisuals), nameof(PlayerAvatarVisuals.Start))]
    private static void Start_Postfix(PlayerAvatarVisuals __instance)
    {
        __instance.gameObject.AddComponent<PlayerAvatarSliders>();
    }

    void Awake()
    {
        playerAvatarVisuals = this.GetComponent<PlayerAvatarVisuals>();
        FindAvatarTransforms();
    }

    void Start()
    {
        if (playerAvatarVisuals.isMenuAvatar)
        {
            MenuAvatarGetSliderSettingsFromRealAvatar();
        }
    }

    void Update()
    {
        if (isDirty)
        {
            ApplyAbsoluteScales();
            isDirty = false;
        }
    }

    void LateUpdate()
    {
        ApplyRelativeScales();
    }

    internal void SetDirty()
    {
        isDirty = true;
    }

    private void FindAvatarTransforms()
    {
        legLBot = this.gameObject.FindFirstChild("ANIM LEG L BOT")?.transform;
        legLTop = this.gameObject.FindFirstChild("ANIM LEG L TOP")?.transform;
        legRBot = this.gameObject.FindFirstChild("ANIM LEG R BOT")?.transform;
        legRTop = this.gameObject.FindFirstChild("ANIM LEG R TOP")?.transform;

        bodyBot      = this.gameObject.FindFirstChild("ANIM BODY BOT")?.transform;
        bodyBotScale = this.gameObject.FindFirstChild("ANIM BODY BOT SCALE")?.transform;
        bodyTop      = this.gameObject.FindFirstChild("ANIM BODY TOP")?.transform;
        bodyTopScale = this.gameObject.FindFirstChild("ANIM BODY TOP SCALE")?.transform;
        
        armL = this.gameObject.FindFirstChild("ANIM ARM L")?.transform;
        armR = this.gameObject.FindFirstChild("ANIM ARM R")?.transform;

        headBot = this.gameObject.FindFirstChild("ANIM HEAD BOT")?.transform;
        headTop = this.gameObject.FindFirstChild("ANIM HEAD TOP")?.transform;

        eyeL = this.gameObject.FindFirstChild("ANIM EYE LEFT")?.transform;
        eyeR = this.gameObject.FindFirstChild("ANIM EYE RIGHT")?.transform;
    }

    private void MenuAvatarGetSliderSettingsFromRealAvatar()
    {
        var realAvatar = playerAvatarVisuals.playerAvatar ?? PlayerAvatar.instance;
        if (realAvatar?.playerAvatarVisuals?.TryGetComponent(out PlayerAvatarSliders realAvatarSliders) ?? false)
        {
            ScaleSettings = realAvatarSliders.ScaleSettings;
        }
        else
        {
            ScaleSettings = UserConfigModel.Instance.GetScaleSettings();
        }
    }

    private void ApplyAbsoluteScales()
    {
        static void Set(Transform? transform, Vector3 scale)
        {
            if (transform == null) return;
            transform.localScale = scale;
        }

        appliedScaleSettings = ScaleSettings.Clamp(
            SemibotSliders.ConfigModel.ScaleMinimum.Value,
            SemibotSliders.ConfigModel.ScaleMaximum.Value
        );

        Set(bodyBot, appliedScaleSettings.BodyBot);
        Set(bodyTop, appliedScaleSettings.BodyTop);
        Set(headBot, appliedScaleSettings.HeadBot);
        Set(headTop, appliedScaleSettings.HeadTop);
        Set(eyeL   , appliedScaleSettings.EyeL   );
        Set(eyeR   , appliedScaleSettings.EyeR   );
        Set(armL   , appliedScaleSettings.ArmL   );
        Set(armR   , appliedScaleSettings.ArmR   );
    }

    private void ApplyRelativeScales()
    {
        static void Set(Transform? transform, Vector3 scale)
        {
            if (transform == null) return;
            var localScale = transform.localScale;
            localScale.Scale(scale);
            transform.localScale = localScale;
        }
        
        Set(legLBot     , appliedScaleSettings.LegLBot     );
        Set(legLTop     , appliedScaleSettings.LegLTop     );
        Set(legRBot     , appliedScaleSettings.LegRBot     );
        Set(legRTop     , appliedScaleSettings.LegRTop     );
        Set(bodyBotScale, appliedScaleSettings.BodyBotScale);
        Set(bodyTopScale, appliedScaleSettings.BodyTopScale);
    }
}