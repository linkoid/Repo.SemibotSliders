using MenuLib;
using MenuLib.MonoBehaviors;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Linkoid.Repo.SemibotSliders;

internal static class MoreREPOTemplates
{
    internal static readonly Transform avatarPreviewTemplate;

    static MoreREPOTemplates()
    {
        foreach (MenuManager.MenuPages menuPage in MenuManager.instance.menuPages)
        {
            Transform transform = menuPage.menuPage.transform;
            MenuPageIndex menuPageIndex = menuPage.menuPageIndex;
            switch (menuPageIndex)
            {
            }
        }
    }

    public static REPOAvatarPreview CreateREPOAvatarPreview(Transform parent, Vector3? localPosition = null)
    {
        Transform transform = Object.Instantiate(MoreREPOTemplates.avatarPreviewTemplate, parent);
        transform.SetParent(parent, true);

        var repoPlayerAvatar = transform.gameObject.AddComponent<REPOAvatarPreview>();

        if (localPosition.HasValue)
        {
            transform.localPosition = localPosition.Value;
        }
        return repoPlayerAvatar;
    }
}

public sealed class REPOAvatarPreview : MonoBehaviour
{
    public PlayerAvatarMenu playerAvatarMenu { get; private set; }
    public PlayerAvatarMenuHover playerAvatarMenuHover { get; private set; }

    void Awake()
    {
        playerAvatarMenu = this.GetComponentInChildren<PlayerAvatarMenu>();
        playerAvatarMenuHover = this.GetComponentInChildren<PlayerAvatarMenuHover>();
    }
}
