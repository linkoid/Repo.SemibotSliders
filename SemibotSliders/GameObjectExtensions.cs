using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Linkoid.Repo.SemibotSliders;

internal static class GameObjectExtensions
{
    public static GameObject? Find(this GameObject parent, string name)
    {
        return parent.transform.Find(name)?.gameObject;
    }

    public static GameObject? FindFirstChild(this GameObject parent, string name)
    {
        return parent.transform.FindFirstChild(name)?.gameObject;
    }

    public static Transform? FindFirstChild(this Transform parent, string name)
    {
        Queue<Transform> queue = new();
        queue.Enqueue(parent);

        while (queue.Count > 0)
        {
            parent = queue.Dequeue();

            var child = parent.Find(name);
            if (child != null)
                return child;

            int childCount = parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                queue.Enqueue(parent.GetChild(i));
            }
        }

        return null;
    }
}
