﻿using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Scripts.Logic
{
    [CreateAssetMenu(menuName = "Game/Fixed animation controller")]
    public class FixedAnimationController : ScriptableObject
    {
        [SerializeField] private FixedAnimation[] _animations;

        private Dictionary<string, int> _nameCache = new();

        public int AnimationCount => _animations.Length;

        public Sprite GetSprite(int index, int frame)
        {
            Debug.Assert(index >= 0 && index < AnimationCount, "Invalid animation index");
            return GetAnimation(index).GetByFrame(frame);
        }

        public Sprite GetSprite(string animationName, int frame) => GetSprite(GetIndex(animationName), frame);

        public int GetIndex(string animationName)
        {
            if (!_nameCache.TryGetValue(animationName, out int index))
            {
                index = Array.FindIndex(_animations, x => x.name == animationName);
                if (index >= 0)
                    _nameCache.Add(animationName, index);
                else
                    throw new KeyNotFoundException($"Animation '{animationName}' not exist.");
            }
        
            return index;
        }

        public FixedAnimation GetAnimation(int index) => _animations[index];

        public FixedAnimation GetAnimation(string animationName) => GetAnimation(GetIndex(animationName));
    }
}