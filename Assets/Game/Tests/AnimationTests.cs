using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


namespace Game.Tests
{
    class AnimationTests : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Animator _animator;
        

        private StatesStream _states = new();
        private IEnumerator<AnimFrame> _enumerator;

        
        private void Start()
        {
            _states.AddIdle(37).AddAttack().AddRun(100);
            _enumerator = _states.GetEnumerator();
        }

        private void Update()
        {
            _enumerator.MoveNext();
            var frame = _enumerator.Current;
            Debug.Log(frame.ToString());

            if (frame.StateFrame == 0)
            {
                PlayState(frame.State);
            }
        }

        private void PlayState(AnimState state)
        {
            switch (state)
            {
            case AnimState.Idle:
                _animator.Play("Idle");
                break;
            case AnimState.Attack:
                _animator.PlayInFixedTime("Attack3", 0, 1 / 6f);
                break;
            case AnimState.Run:
                _animator.Play("Run");
                break;
            default:
                _animator.Play("Idle");
                break;
            }
        }
    }
}
