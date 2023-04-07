using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = System.Random;


namespace Game.Tests
{
    class AnimationTests : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private FixedAnimation _attackClip;
        [SerializeField] private FixedAnimation _idleClip;
        [SerializeField] private FixedAnimation _runClip;

        [SerializeField] private AnimState _state;
        [SerializeField] private int _currentFrame;
        [SerializeField] private int _totalFrame;
        
        private StatesStream _states = new();
        private IEnumerator<AnimFrame> _enumerator;

        private void Start()
        {
            _states.AddIdle(60).AddAnimState(AnimState.Attack, 30).AddRun(120);
            //_states.AddAnimState(AnimState.Attack, 30).AddAnimState(AnimState.Idle, 1);
            //_enumerator = _states.GetEnumerator();
            _enumerator = _states.GetEnumerator(new Random(512789461), 0.02f, -20, 20);
        }

        private void Update()
        {
            _enumerator.MoveNext();
            var state = _enumerator.Current;
            _state = state.State;
            _currentFrame = state.StateFrame;
            _totalFrame = state.TotalFrame;
            
            PlayAnimation(state, state.StateFrame);
        }

        private void PlayAnimation(AnimFrame state, int startFrame)
        {
            switch (state.State)
            {
            case AnimState.Idle:
                Play(_idleClip, startFrame);
                break;
            case AnimState.Attack:
                Play(_attackClip, startFrame);
                break;
            case AnimState.Run:
                Play(_runClip, startFrame);
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private void Play(FixedAnimation clip, int frame)
        {
            _renderer.sprite = clip.Get(frame);
        }
    }
}
