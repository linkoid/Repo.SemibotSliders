using HarmonyLib;
using MenuLib;
using MenuLib.MonoBehaviors;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Linkoid.Repo.SemibotSliders;

internal class REPOVector3SliderModel //: REPOElement
{
    public Action<Vector3>? onValueChanged;

    private Vector3 defaultValue;
    private REPOLabel label = null!;
    private REPOSlider xSlider = null!;
    private REPOSlider ySlider = null!;
    private REPOSlider zSlider = null!;

    public Vector3 min
    {
        get => _min;
        set
        {
            _min = value;
            xSlider.min = _min.x;
            ySlider.min = _min.y;
            zSlider.min = _min.z;
        }
    }
    private Vector3 _min = Vector3.zero;

    public Vector3 max
    {
        get => _max;
        set
        {
            _max = value;
            xSlider.max = _max.x;
            ySlider.max = _max.y;
            zSlider.max = _max.z;
        }
    }
    private Vector3 _max = Vector3.one;

    public int precision
    { 
        get => _precision;
        set
        {
            _precision = value;
            xSlider.precision = _precision;
            ySlider.precision = _precision;
            zSlider.precision = _precision;
        }
    }
    private int _precision = 2;


    public float precisionDecimal
    {
        get => _precisionDecimal;
        set
        {
            _precisionDecimal = value;
            xSlider.precisionDecimal = _precisionDecimal;
            ySlider.precisionDecimal = _precisionDecimal;
            zSlider.precisionDecimal = _precisionDecimal;
        }
    }
    private float _precisionDecimal = 0.01f;

    public Vector3 value
    {
        get => new Vector3(xSlider.value, ySlider.value, zSlider.value);
        set
        {
            xSlider.@value = value.x;
            ySlider.@value = value.y;
            zSlider.@value = value.z;
        }
    }

    //public static REPOVector3Slider Create(string text, string description, Action<Vector3> onValueChanged, Transform parent, 
    //    Vector3 min = default, Vector3? max = null, int precision = 2, Vector3 defaultValue = default)
    //{
    //    max ??= Vector3.one;

    //    GameObject gameObject = FrameElement.Create(nameof(REPOVector3Slider), parent).gameObject;
    //    VerticalLayoutGroup verticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
    //    REPOVector3Slider vector3Slider = gameObject.AddComponent<REPOVector3Slider>();

    //    var subparent = vector3Slider.transform;
    //    vector3Slider.label = MenuAPI.CreateREPOLabel(text, subparent);
    //    vector3Slider.xSlider = MenuAPI.CreateREPOSlider("x", ""         , vector3Slider.OnInternalValueChanged, subparent, default, min.x, max.Value.x, precision, defaultValue.x);
    //    vector3Slider.ySlider = MenuAPI.CreateREPOSlider("y", ""         , vector3Slider.OnInternalValueChanged, subparent, default, min.y, max.Value.y, precision, defaultValue.y);
    //    vector3Slider.zSlider = MenuAPI.CreateREPOSlider("z", description, vector3Slider.OnInternalValueChanged, subparent, default, min.z, max.Value.z, precision, defaultValue.z);
    //    vector3Slider.onValueChanged = onValueChanged;

    //    vector3Slider.min = min;
    //    vector3Slider.max = max.Value;
    //    vector3Slider.precision = precision;

    //    vector3Slider.SetValue(defaultValue, invokeCallback: false);

    //    return vector3Slider;
    //}

    public REPOVector3SliderModel(string text, string description, Action<Vector3> onValueChanged, Transform parent,
        Vector3 min = default, Vector3? max = null, int precision = 2, Vector3 defaultValue = default)
    {
        max ??= Vector3.one;

        label = MenuAPI.CreateREPOLabel(text, parent);
        xSlider = MenuAPI.CreateREPOSlider("x", ""         , OnInternalValueChanged, parent, default, min.x, max.Value.x, precision, defaultValue.x);
        ySlider = MenuAPI.CreateREPOSlider("y", ""         , OnInternalValueChanged, parent, default, min.y, max.Value.y, precision, defaultValue.y);
        zSlider = MenuAPI.CreateREPOSlider("z", description, OnInternalValueChanged, parent, default, min.z, max.Value.z, precision, defaultValue.z);
        this.onValueChanged = onValueChanged;

        this.min = min;
        this.max = max.Value;
        this.precision = precision;

        SetValue(defaultValue, invokeCallback: false);
    }

    public REPOVector3SliderModel(string text, string description, Action<Vector3> onValueChanged, REPOScrollView scrollView,
        Vector3 min = default, Vector3? max = null, int precision = 2, Vector3 defaultValue = default)
        : this(text, description, onValueChanged, scrollView.popupPage.menuScrollBox.scroller, min, max, precision, defaultValue)
    {
        scrollView.popupPage.AddElementToScrollView(label  .rectTransform, label  .rectTransform.localPosition);
        scrollView.popupPage.AddElementToScrollView(xSlider.rectTransform, xSlider.rectTransform.localPosition);
        scrollView.popupPage.AddElementToScrollView(ySlider.rectTransform, ySlider.rectTransform.localPosition);
        scrollView.popupPage.AddElementToScrollView(zSlider.rectTransform, zSlider.rectTransform.localPosition);
    }

    public void SetValue(Vector3 newValue, bool invokeCallback)
    {
        xSlider.SetValue(newValue.x, invokeCallback);
        ySlider.SetValue(newValue.y, invokeCallback);
        zSlider.SetValue(newValue.z, invokeCallback);
    }

    //private void OnXValueChanged(float value) { currentValue.x = value; OnInternalValueChanged(); }
    //private void OnYValueChanged(float value) { currentValue.y = value; OnInternalValueChanged(); }
    //private void OnZValueChanged(float value) { currentValue.z = value; OnInternalValueChanged(); }

    private void OnInternalValueChanged(float _)
    {
        onValueChanged?.Invoke(value);
    }
}
