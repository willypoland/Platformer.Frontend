using System;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Fixed animation controller")]
public class FixedAnimationController : ScriptableObject
{
    [SerializeField] private FixedAnimation[] _animations;

    private Dictionary<string, int> _nameCache = new();

    public int AnimationCount => _animations.Length;

    public Sprite GetSprite(int index, int frame)
    {
        Debug.Assert(index >= 0 && index < AnimationCount, "Invalid animation index");
        return _animations[index].Get(frame);
    }

    public Sprite GetSprite(string animationName, int frame)
    {
        int index = GetIndex(animationName);
        return GetSprite(index, frame);
    }

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
    
    public FixedAnimation GetAnimation(string animationName) => _animations[GetIndex(animationName)];
}