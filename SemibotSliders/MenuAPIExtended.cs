using HarmonyLib;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using UnityEngine;
using static MenuLib.MenuAPI;

namespace Linkoid.Repo.SemibotSliders;

internal static class MenuAPIExtended
{
    private static readonly Dictionary<Type, BuilderDelegate> sharedMenuBuilderDelegates = new();

    public static void AddElementTo<T>(BuilderDelegate builderDelegate)
        where T : MonoBehaviour
    {
        Type menuPageType = typeof(T);
        bool hadSharedDelegate = sharedMenuBuilderDelegates.TryGetValue(menuPageType, out var sharedDelegate);
        sharedDelegate += builderDelegate;

        if (hadSharedDelegate) return;

        // else

        var startMethod = AccessTools.Method(menuPageType, "Start")
            ?? throw new InvalidOperationException($"Cannot add element to type `{menuPageType.Name}` (has no Start method)");

        void MenuPage_StartHook(Action<T> orig, T self)
        {
            orig.Invoke(self);
            sharedDelegate?.Invoke(self.transform);
        }

        SemibotSliders.Logger.LogDebug($"Hooking `{menuPageType.Name}.Start`");
        new Hook(startMethod, MenuPage_StartHook);

        sharedMenuBuilderDelegates.Add(menuPageType, sharedDelegate);
    }
}
