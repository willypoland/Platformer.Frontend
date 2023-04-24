using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtendedSlider : MonoBehaviour
{
    public delegate void ValueChangedEvent(float value, float min, float max);
    
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private float _value = 0f;
    [SerializeField] private float _min = 0f;
    [SerializeField] private float _max = 100f;
    [SerializeField] private string _format = "f0";
    [Header("Helper")] 
    [SerializeField, Min(60)] private int _inputWidth = 100;

    public event ValueChangedEvent ValueChanged;

    public float Min => _min;
    public float Max => _max;
    public float Value => _value;
    public float Fraction => Mathf.InverseLerp(_min, _max, _value);

    private void OnValidate()
    {
        _input.GetComponent<LayoutElement>().minWidth = _inputWidth;
    }

    private void Awake()
    {
        _slider.minValue = 0f;
        _slider.maxValue = 1f;
        _slider.value = Fraction;
        
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        _input.onDeselect.AddListener(OnInputFieldValueChanged);
        _input.onSubmit.AddListener(OnInputFieldValueChanged);
        UpdateFields(_value, _min, _max);
    }

    public void Set(float value, float min, float max)
    {
        if (value != _value || _min != min || _max != max)
            UpdateFields(value, min, max);
    }

    private void UpdateFields(float value, float min, float max)
    {
        max = Mathf.Max(min, max);
        value = Mathf.Clamp(value, min, max);

        _value = value;
        _min = min;
        _max = max;

        _slider.SetValueWithoutNotify(Fraction);
        _input.SetTextWithoutNotify(_value.ToString(_format, CultureInfo.InvariantCulture));
        ValueChanged?.Invoke(_value, _min, _max);
    }

    private void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
        {
            if (_value != result) 
                UpdateFields(result, _min, _max);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        float result = Mathf.Lerp(_min, _max, value);
        if (_value != result)
            UpdateFields(result, _min, _max);
    }

}
