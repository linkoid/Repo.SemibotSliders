using Sirenix.Utilities;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Linkoid.Repo.SemibotSliders;

[System.Serializable]
internal struct ScaleSettings
{
    public static readonly ScaleSettings Default = new ScaleSettings()
    {
        LegLBot      = Vector3.one,
        LegLTop      = Vector3.one,
        LegRBot      = Vector3.one,
        LegRTop      = Vector3.one,
        BodyBot      = Vector3.one,
        BodyBotScale = Vector3.one,
        BodyTop      = Vector3.one,
        BodyTopScale = Vector3.one,
        ArmL         = Vector3.one,
        ArmR         = Vector3.one,
        HeadBot      = Vector3.one,
        HeadTop      = Vector3.one,
        EyeL         = Vector3.one,
        EyeR         = Vector3.one,
    };

    public Vector3 LegLBot;
    public Vector3 LegLTop;
    public Vector3 LegRBot;
    public Vector3 LegRTop;
    public Vector3 BodyBot;
    public Vector3 BodyBotScale;
    public Vector3 BodyTop;
    public Vector3 BodyTopScale;
    public Vector3 ArmL;
    public Vector3 ArmR;
    public Vector3 HeadBot;
    public Vector3 HeadTop;
    public Vector3 EyeL;
    public Vector3 EyeR;

    public ScaleSettings Clamp(float min, float max)
    {
        return Clamp(Vector3.one * min, Vector3.one * max);
    }

    public ScaleSettings Clamp(Vector3 min, Vector3 max)
    {
        return new ScaleSettings()
        {
            LegLBot      = LegLBot     .Clamp(min, max),
            LegLTop      = LegLTop     .Clamp(min, max),
            LegRBot      = LegRBot     .Clamp(min, max),
            LegRTop      = LegRTop     .Clamp(min, max),
            BodyBot      = BodyBot     .Clamp(min, max),
            BodyBotScale = BodyBotScale.Clamp(min, max),
            BodyTop      = BodyTop     .Clamp(min, max),
            BodyTopScale = BodyTopScale.Clamp(min, max),
            ArmL         = ArmL        .Clamp(min, max),
            ArmR         = ArmR        .Clamp(min, max),
            HeadBot      = HeadBot     .Clamp(min, max),
            HeadTop      = HeadTop     .Clamp(min, max),
            EyeL         = EyeL        .Clamp(min, max),
            EyeR         = EyeR        .Clamp(min, max),
        };
    }

    public Vector3[] ToArray()
    {
        return new[]
        {
            LegLBot,
            LegLTop,
            LegRBot,
            LegRTop,
            BodyBot,
            BodyBotScale,
            BodyTop,
            BodyTopScale,
            ArmL,
            ArmR,
            HeadBot,
            HeadTop,
            EyeL,
            EyeR,
        };
    }

    public static ScaleSettings FromArray(Vector3[] array)
    {
        int i = 0;
        return new ScaleSettings()
        {
            LegLBot      = array[i++],
            LegLTop      = array[i++],
            LegRBot      = array[i++],
            LegRTop      = array[i++],
            BodyBot      = array[i++],
            BodyBotScale = array[i++],
            BodyTop      = array[i++],
            BodyTopScale = array[i++],
            ArmL         = array[i++],
            ArmR         = array[i++],
            HeadBot      = array[i++],
            HeadTop      = array[i++],
            EyeL         = array[i++],
            EyeR         = array[i++],
        };
    }
}


