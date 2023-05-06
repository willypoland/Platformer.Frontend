using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;



namespace Game.Scripts.Logic
{
    public class FpsPlot : MonoBehaviour
    {
        [SerializeField] private PlotView _view;
        [SerializeField] private float _minMaxLerpSpeed = 1f;
        [SerializeField] private float _minMaxOffsetMul = 0.1f;
        private Texture2D _texture;

        private float _currentMin;
        private float _currentMax;

        private void Update()
        {
            if (_view == null)
                return;
            
            _view.Push(Time.deltaTime);

            float min = _view.Queue.Min;
            float max = _view.Queue.Max;
            
            _currentMax = Mathf.Max(_currentMax, max);
            _currentMin = Mathf.Min(_currentMin, min);

            float speed = Time.deltaTime * _minMaxLerpSpeed;
            float offset = _view.Queue.Avg * _minMaxOffsetMul; 
            _currentMin = Mathf.Lerp(_currentMin, min, speed) - offset;
            _currentMax = Mathf.Lerp(_currentMax, max, speed) + offset;
            
            _view.SetMinMax(_currentMin, _currentMax);
        }
    }
}
