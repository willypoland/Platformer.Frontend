﻿using System;
using UnityEngine;


namespace Game
{
    [CreateAssetMenu(menuName = "Game/Fixed animation")]
    public class FixedAnimation : ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField, Min(1)] private int _timeInFrames = 1;
        [SerializeField] private bool _repeate;

        private float _inputStep;
        
        private void OnValidate()
        {
            _inputStep = 1f / _timeInFrames;
        }

        public int SpriteCount => _sprites.Length;

        public int TimeInFrames => _timeInFrames;

        public bool Repeate => _repeate;

        public Sprite Get(int frame) => _sprites[MapIndex(frame)];

        public int MapIndex(int frame) => Mathf.FloorToInt((frame % _timeInFrames) * _inputStep * _sprites.Length);

#if UNITY_EDITOR
        public static FixedAnimation Mock(int sprites, int timeInFrames, bool repeate)
        {
            var so = CreateInstance<FixedAnimation>();
            so._sprites = new Sprite[sprites];
            so._repeate = repeate;
            so._timeInFrames = timeInFrames;
            so.OnValidate();
            return so;
        }
#endif
    }
}