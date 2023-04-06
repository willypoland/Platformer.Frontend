using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Tests
{
    class FramedAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;

        [Space]
        [SerializeField] private FramedAnimationClip _activeClip;
        [SerializeField] private int _spriteIndex = 0;
        [SerializeField] private float _time = 0f;
        [SerializeField] private float _timePerSprite = 0f;
        
        [SerializeField] private bool _playing;
        
        private void Reset()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public bool Playing => _playing;
        public int SpriteIndex => _spriteIndex;
        public float CurrentTime => _time;
        public float CurrentFrameTimeOffset => _playing ? _time % _timePerSprite : 0f;
        public float TimePerSprite => _timePerSprite;
        
        public void RePlay(FramedAnimationClip clip, int startFrame = 0, float frameTimeOffset = 0)
        {
            _playing = true;
            _activeClip = clip;
            _timePerSprite = clip.Count > 1 ? clip.Time / clip.Count : clip.Time; 
            _spriteIndex = startFrame;
            _time = startFrame * _timePerSprite + frameTimeOffset;
            _spriteIndex = GetSpriteIndex();
        }

        public void Play(FramedAnimationClip clip, int frame)
        {
            if (!_playing || !ReferenceEquals(_activeClip, clip))
            {
                Debug.Log("Full replay");
                RePlay(clip, frame, 0);
            }
            else if (GetSpriteIndex() != frame)
            {
                Debug.Log("Change ");
                RePlay(clip, frame, CurrentFrameTimeOffset);
            }
        }

        public void Stop()
        {
            ClearState();
        }

        private void Update()
        {
            if (_playing)
            {
                PlayAnimation();
            }
        }

        private void PlayAnimation()
        {
            _renderer.sprite = _activeClip.Get(_spriteIndex);
            _time += Time.deltaTime;

            if (ShouldStopPlaying())
                ClearState();
            else
                _spriteIndex = GetSpriteIndex();
        }

        private bool ShouldStopPlaying()
        {
            return !_activeClip.Repeate && _time > _activeClip.Time;
        }

        private void ClearState()
        {
            _playing = false;
            _activeClip = null;
            _time = 0;
            _spriteIndex = 0;
            _timePerSprite = 0;
        }

        private int GetSpriteIndex() => Mathf.FloorToInt(_time / _timePerSprite);
    }
}