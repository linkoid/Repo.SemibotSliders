using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Linkoid.Repo.SemibotSliders;

[HarmonyPatch]
internal class PlayerAvatarSlidersSync : MonoBehaviour
{
    private PhotonView photonView = null!;
    private PlayerAvatar playerAvatar = null!;
    private PlayerAvatarSliders playerAvatarSliders = null!;

    private bool isDirty = false; 
    private ScaleSettings scaleSettings = ScaleSettings.Default;

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.Awake))]
    private static void Awake_Postfix(PlayerAvatar __instance)
    {
        __instance.gameObject.AddComponent<PlayerAvatarSlidersSync>();
    }

    void Awake()
    {
        photonView = this.GetComponent<PhotonView>();
        playerAvatar = this.GetComponent<PlayerAvatar>();
    }

    void Start()
    {
        playerAvatarSliders = playerAvatar.playerAvatarVisuals.GetComponent<PlayerAvatarSliders>();

        if (!SemiFunc.IsMultiplayer() || photonView.IsMine)
        {
            this.StartCoroutine(WaitForSteamID());
        }
    }

    void Update()
    {
        ApplyScaleSettingsIfDirty();
    }

    private IEnumerator WaitForSteamID()
    {
        while (playerAvatar.steamID == null) yield return null;

        if (SemiFunc.IsMultiplayer() || !SemiFunc.IsMainMenu())
        {
            var scaleSettings = UserConfigModel.Instance.GetScaleSettings();
            SemibotSliders.Logger.LogDebug(scaleSettings);
            SetScaleSettings(scaleSettings);
        }
    }

    public void SetScaleSettings(ScaleSettings scaleSettings)
    {
        if (!GameManager.Multiplayer())
        {
            SemibotSliders_SetScaleSettingsRPC(scaleSettings.ToArray());
        }
        else
        {
            photonView.RPC(nameof(SemibotSliders_SetScaleSettingsRPC), RpcTarget.AllBuffered, scaleSettings.ToArray());
        }
    }

    [PunRPC, MethodImpl(MethodImplOptions.NoInlining)]
    public void SemibotSliders_SetScaleSettingsRPC(Vector3[] scaleSettingsArray)
    {
        scaleSettings = ScaleSettings.FromArray(scaleSettingsArray);
        isDirty = true;
    }

    private void ApplyScaleSettingsIfDirty()
    {
        if (!isDirty) return;
        if (playerAvatarSliders == null) return;

        playerAvatarSliders.ScaleSettings = scaleSettings;
        isDirty = false;
    }
}