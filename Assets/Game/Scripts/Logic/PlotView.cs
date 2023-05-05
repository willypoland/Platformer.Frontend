using UnityEngine;
using UnityEngine.UI;


public class PlotView : Graphic
{
    private static readonly int PlotTex = Shader.PropertyToID("_PlotTex");
    private static readonly int Min = Shader.PropertyToID("_Min");
    private static readonly int Max = Shader.PropertyToID("_Max");

    [SerializeField] private int _initWidth = 200;
    [SerializeField] private float _minMaxLerpSpeed = 1f; 
    private Texture2D _texture;

    private float _min;
    private float _max;
    private float[] _buffer;

    private float _currentMin;
    private float _currentMax;

    protected override void Awake()
    {
        base.Awake();
        SetWidth(_initWidth);
    }

    public void SetWidth(int width)
    {
        _min = float.PositiveInfinity;
        _max = float.NegativeInfinity;
        
        if (Application.isEditor)
            DestroyImmediate(_texture);
        else
            Destroy(_texture);


        _buffer = new float[width];
        _texture = new Texture2D(width, 1, TextureFormat.RFloat, 0, false);
        _texture.name = "PlotTexture";
        _texture.filterMode = FilterMode.Point;
        _texture.SetPixelData(_buffer, 0);
        _texture.Apply(false);

        material.SetTexture(PlotTex, _texture);
        (transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public void Push(float value)
    {
        _min = _max = value;
        
        for (int i = 1; i < _buffer.Length; i++)
        {
            float v = _buffer[i];
            _min = Mathf.Min(v, _min);
            _max = Mathf.Max(v, _max);
            
            _buffer[i - 1] = v;
        }

        _buffer[^1] = value;

        _texture.SetPixelData(_buffer, 0);
        _texture.Apply(false);
    }

    private void Update()
    {
        Push(Time.deltaTime);

        _currentMax = Mathf.Max(_currentMax, _max);
        _currentMin = Mathf.Min(_currentMin, _min);

        float speed = Time.deltaTime * _minMaxLerpSpeed;
        _currentMin = Mathf.Lerp(_currentMin, _min, speed);
        _currentMax = Mathf.Lerp(_currentMax, _max, speed);
        
        material.SetFloat(Min, _currentMin);
        material.SetFloat(Max, _currentMax);
    }
}
