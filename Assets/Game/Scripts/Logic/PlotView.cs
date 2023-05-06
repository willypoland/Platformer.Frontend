using UnityEngine;
using UnityEngine.UI;


namespace Game.Scripts.Logic
{
    public class PlotView : Graphic
    {
        private static readonly int PlotTex = Shader.PropertyToID("_PlotTex");
        private static readonly int Min = Shader.PropertyToID("_Min");
        private static readonly int Max = Shader.PropertyToID("_Max");

        private readonly FloatQueue _queue = new();
        private Texture2D _texture;

        public FloatQueue Queue => _queue;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Resize(Mathf.RoundToInt(rectTransform.sizeDelta.x));
        }
        
        private void Resize(int size)
        {
            _queue.Resize(size);
            _texture = new Texture2D(size, 1, TextureFormat.RFloat, 0, false);
            _texture.name = "PlotTexture." + name;
            _texture.filterMode = FilterMode.Point;
            _texture.SetPixelData(_queue.Buffer, 0);
            _texture.Apply(false);

            material.SetTexture(PlotTex, _texture);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
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
            material.SetFloat(Min, min);
            material.SetFloat(Max, max);
        }
    }
}