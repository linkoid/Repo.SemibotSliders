using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linkoid.Repo.SemibotSliders;

internal record ModConfigModel(ConfigFile ConfigFile)
{
    public const string GeneralSliderConstraintsSection = "GeneralSliderConstraints";
    public readonly ConfigEntry<float> ScaleMinimum = ConfigFile.Bind(GeneralSliderConstraintsSection, "ScaleMinimum", 0f, new ConfigDescription("Smallest applyable scale factor for body sliders", new AcceptableValueRange<float>(0.00f, 0.99f)));
    public readonly ConfigEntry<float> ScaleMaximum = ConfigFile.Bind(GeneralSliderConstraintsSection, "ScaleMaximum", 3f, new ConfigDescription("Largest applyable scale factor for body sliders" , new AcceptableValueRange<float>(1.01f, 3.00f)));
}
