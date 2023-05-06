using System;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class FloatQueue
    {
        private float _min;
        private float _max;
        private float _avg;
        private float _sum;
        private float[] _buffer;

        public int Size => _buffer.Length;
        public float Min => _min;
        public float Max => _max;
        public float Avg => _avg;
        public float Sum => _sum;
        public float[] Buffer => _buffer;

        public void Resize(int size)
        {
            _min = _max = _avg = _sum = 0;
            Array.Resize(ref _buffer, size);
        }

        public void Push(float value)
        {
            _min = _max = value;

            _sum = 0;
            for (int i = 1; i < _buffer.Length; i++)
            {
                float v = _buffer[i];
                _sum += v;
                _min = Mathf.Min(v, _min);
                _max = Mathf.Max(v, _max);
                _buffer[i - 1] = v;
            }

            _avg = _sum / _buffer.Length;
            _buffer[^1] = value;
        }
    }
}