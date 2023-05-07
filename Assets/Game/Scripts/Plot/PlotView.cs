using System;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Scripts.Plot
{
    public class PlotView : Graphic
    {
        private static readonly int IdPlotTex = Shader.PropertyToID("_PlotTex");
        private static readonly int IdMin = Shader.PropertyToID("_Min");
        private static readonly int IdMax = Shader.PropertyToID("_Max");
        private static readonly int IdColor = Shader.PropertyToID("_Color");

        private readonly FloatQueue _queue = new();
        private Texture2D _texture;

        public FloatQueue Queue => _queue;

        public int Width => Mathf.RoundToInt(rectTransform.rect.width);

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Resize(Width);
        }

        protected override void Awake()
        {
            base.Awake();
            Resize(Width);
            
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            color = base.color;
        }

        private void SetupMaterial()
        {
            if (material.shader.name != "Game/Plot")
            {
                var shader = Shader.Find("Game/Plot");
                material = new Material(shader);
                material.name = "PlotMaterial." + name;
            }
        }

        public override Color color
        {
            get => base.color;
            set
            {
                base.color = value;
                material.SetColor(IdColor, value);
            }
        }

        private void Resize(int size)
        {
            if (size < 1 || _queue.Size == size)
                return;
            
            _queue.Resize(size);
            
            DestroyTexture();
            SetupMaterial();
            
            _texture = new Texture2D(size, 1, TextureFormat.RFloat, 0, false);
            _texture.name = "PlotTexture." + name;
            _texture.filterMode = FilterMode.Point;
            _texture.SetPixelData(_queue.Buffer, 0);
            _texture.Apply(false);

            material.SetTexture(IdPlotTex, _texture);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            
            SetMaterialDirty();
        }

        public void Push(float value, bool updateMinMax = false)
        {
            _queue?.Push(value);

            if (_texture)
            {
                _texture.SetPixelData(_queue.Buffer, 0);
                _texture.Apply(false);
            }

            if (updateMinMax)
                SetMinMax(_queue.Min, _queue.Max);
        }

        
        public void SetMinMax(float min, float max)
        {
            min = Mathf.Min(min, max);
            material.SetFloat(IdMin, min);
            material.SetFloat(IdMax, max);
        }

        private void DestroyTexture()
        {
            if (_texture == null)
                return;
            
            if (Application.isEditor)
                DestroyImmediate(_texture);
            else
                Destroy(_texture);
        }
    }
}