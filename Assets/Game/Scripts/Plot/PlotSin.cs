using System;
using UnityEngine;


namespace Game.Scripts.Plot
{
    [RequireComponent(typeof(PlotView))]
    public class PlotSin : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        private PlotView _view;

        private void Awake() => _view = GetComponent<PlotView>();

        private void Start()
        {
            _view.SetMinMax(-1f, 1f);
        }

        private void Update()
        {
            if (_view == null)
                return;
            
            _view.Push(Mathf.Sin(Time.time * _speed));
        }
    }
}