using System;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;


namespace Game.Scripts.Plot
{
    [RequireComponent(typeof(PlotView))]
    public class PlotTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _min;
        [SerializeField] private TMP_Text _max;
        [SerializeField] private TMP_Text _avg;
        [SerializeField] private TMP_Text _last;
        [SerializeField] private string _template;
        
        private PlotView _view;
        private StringBuilder _sb = new(32);
        private char[] _buffer = new char[64];

        private void Awake()
        {
            _view = GetComponent<PlotView>();
        }

        private void Update()
        {
            Setup(_min, _view.Queue.Min);
            Setup(_max, _view.Queue.Max);
            Setup(_avg, _view.Queue.Avg);
            Setup(_last, _view.Queue.Buffer[^1]);
        }

        private void Setup(TMP_Text text, float value)
        {
            text.SetText(_template, value);
        }
    }
}