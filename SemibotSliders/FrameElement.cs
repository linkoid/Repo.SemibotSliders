using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Linkoid.Repo.SemibotSliders;

internal static class FrameElement
{
    public static RectTransform Create(string name, Transform parent)
    {
        GameObject gameObject = new GameObject(name, typeof(RectTransform));
        gameObject.layer = LayerMask.NameToLayer("UI");

        RectTransform rectTransform = (RectTransform)gameObject.transform;
        rectTransform.pivot = new(0, 0);
        rectTransform.anchorMin = new(0, 0);
        rectTransform.anchorMax = new(0, 0);
        rectTransform.offsetMin = new(0, 0);
        rectTransform.offsetMax = new(0, 0);
        rectTransform.anchoredPosition3D = new(0, 0, 0);

        rectTransform.SetParent(parent, false);

        return rectTransform;
    }
}
