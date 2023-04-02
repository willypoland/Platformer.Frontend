using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


namespace Game.Tests
{

    class FramedAnimationTest : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private FramedAnimationClip _clipIdle;
        [SerializeField] private FramedAnimationClip _clipRun;
        [SerializeField] private FramedAnimationClip _clipAttack;
        

        private StatesStream _states = new();
        private IEnumerator<AnimFrame> _enumerator;
        
        private void Start()
        {
            _states.AddIdle(7).AddAttack().AddRun(18);
            _enumerator = _states.GetEnumerator();
        }

        private void Update()
        {
            _enumerator.MoveNext();
            var frame = _enumerator.Current;
            Debug.Log(frame.ToString());

            Sprite current = frame.State switch
            {
                AnimState.Idle => _clipIdle.Get(frame.StateFrame),
                AnimState.Run => _clipRun.Get(frame.StateFrame),
                AnimState.Attack => _clipAttack.GetLerp(frame.StateFrame, 5),
                _ => null
            };

            if (current != null)
                _renderer.sprite = current;
        }

        private static Sprite Get(FramedAnimationClip clip, AnimFrame frame) =>
            clip.GetLerp(frame.StateFrame, clip.Count);
    }
}
