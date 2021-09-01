using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsWindget : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    //[SerializeField] private Text _value;

    private FloatPersistentProperty _model;

    private readonly CompositeDisposable _trash = new CompositeDisposable();

    private void Start()
    {
        _trash.Retain(_slider.onValueChanged.Subscribe(OnSliderValueChanged));
    }

    private void OnSliderValueChanged(float value)
    {
        _model.Value = value;
    }

    public void SetModel(FloatPersistentProperty model)
    {
        _model = model;
        _trash.Retain(model.Subscribe(OnValueChanged));
        OnValueChanged(model.Value, model.Value);
    }

    private void OnValueChanged(float newValue, float oldValue)
    {
        var textValue = Mathf.Round(newValue * 100);
        //_value.text = textValue.ToString();
        _slider.normalizedValue = newValue;
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
