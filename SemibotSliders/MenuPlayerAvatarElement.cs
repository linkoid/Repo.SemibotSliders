using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Linkoid.Repo.SemibotSliders;

internal static class MenuPlayerAvatarElement
{
    public static GameObject Create(string name, Transform parent, PlayerAvatarMenu ? playerAvatarMenu = null, RenderTexture? renderTextureAvatar = null, Transform? pointer = null)
    {
        playerAvatarMenu ??= PlayerAvatarMenu.instance;
        renderTextureAvatar ??= playerAvatarMenu?.cameraAndStuff.GetComponentInChildren<Camera>()?.targetTexture;
        pointer ??= playerAvatarMenu?.cameraAndStuff?.transform.Find("Pointer Canvas")?.Find("pointer");

        GameObject gameObject = new GameObject(name);
        gameObject.transform.position = new Vector3(-76f, -30f, 0f);
        gameObject.transform.rotation = new Quaternion(0f, 1f, 0f, 0f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.transform.SetParent(parent, true);

        GameObject textureObject = new GameObject("Player Avatar Render Texture",
            typeof(RectTransform), typeof(CanvasRenderer), typeof(PlayerAvatarMenuHover), typeof(RawImage), typeof(MenuElementHover));
        textureObject.transform.position = new Vector3(424f, 6.04f, 0f);
        textureObject.transform.SetParent(gameObject.transform, true);
        textureObject.transform.localPosition = new Vector3(-500f, 36.04f, 0f);

        RectTransform rectTransform = textureObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(0f, 0f);
        rectTransform.pivot = new Vector2(0f, 0f);
        rectTransform.offsetMin = new Vector2(-500f, 36.04f);
        rectTransform.offsetMax = new Vector2(-316f, 381.04f);
        rectTransform.anchoredPosition = new Vector2(-500f, 36.04f);

        PlayerAvatarMenuHover playerAvatarMenuHover = textureObject.GetComponent<PlayerAvatarMenuHover>();
        playerAvatarMenuHover.pointer = pointer;
        playerAvatarMenuHover.playerAvatarMenu = playerAvatarMenu;

        RawImage rawImage = textureObject.GetComponent<RawImage>();
        rawImage.texture = renderTextureAvatar;
        rawImage.uvRect = new Rect(0, 0, 1, 1);

        MenuElementHover menuElementHover = textureObject.GetComponent<MenuElementHover>();
        menuElementHover.buttonPitch = 0.5375f;
        menuElementHover.hasHoverEffect = false;

        return gameObject;
    }
}
