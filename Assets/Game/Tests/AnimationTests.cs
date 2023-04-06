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
        [SerializeField] private FramedAnimator _animator;
        [SerializeField] private FramedAnimationClip _attackClip;
        [SerializeField] private FramedAnimationClip _idleClip;
        [SerializeField] private FramedAnimationClip _runClip;

        private StatesStream _states = new();
        private IEnumerator<AnimFrame> _enumerator;
        private int _prevFrame;

        private void Start()
        {
            //_states.AddIdle(37).AddAnimState(AnimState.Attack, 30).AddRun(100);
            _states.AddAnimState(AnimState.Attack, 30).AddAnimState(AnimState.Idle, 1);
            _enumerator = _states.GetEnumerator(new Random(512789461), 0.1f, 3, 20);
        }

        private void Update()
        {
            _enumerator.MoveNext();
            var state = _enumerator.Current; 
            if (state.StateFrame == 0 || state.TotalFrame < _prevFrame)
            {
                Debug.Log($"Change: {state.StateFrame}");
                PlayAnimation(state, state.StateFrame);
            }

            _prevFrame = state.TotalFrame;
        }

        private void PlayAnimation(AnimFrame state, int startFrame)
        {
            switch (state.State)
            {
            case AnimState.Idle:
                _animator.Play(_idleClip, startFrame);
                break;
            case AnimState.Attack:
                _animator.Play(_attackClip, startFrame);
                break;
            case AnimState.Run:
                _animator.Play(_runClip, startFrame);
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
