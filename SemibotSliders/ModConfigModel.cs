using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linkoid.Repo.SemibotSliders;

internal record ModConfigModel(ConfigFile ConfigFile)
{
    public const string GeneralSliderConstraintsSection = "GeneralSliderConstraints";
    public readonly ConfigEntry<float> SliderMinimum = ConfigFile.Bind(GeneralSliderConstraintsSection, "SliderMinimum", 0.01f, new ConfigDescription("Smallest applyable scale factor for body sliders", new AcceptableValueRange<float>(0f, 1f)));
    public readonly ConfigEntry<float> SliderMaximum = ConfigFile.Bind(GeneralSliderConstraintsSection, "SliderMaximum", 0.01f, new ConfigDescription("Largest applyable scale factor for body sliders", new AcceptableValueRange<float>(1f, 3f)));
}
