using System;
using Game.Code.Data;
using UnityEngine;


namespace Game.Code.Scene
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private FixedAnimationController _animator;
        [SerializeField] private GameObject _interpolactionIndicator;

        private Vector3 _curPosition;
        private Vector3 _lastPosition;
        private GameConfig _config;

        public void Set(Api.IPlayer player, GameConfig config)
        {
            _config = config;
            _lastPosition = _curPosition;
            _curPosition = player.Object.Position + player.Object.Size * 0.5f;

            _renderer.flipX = player.LeftDireciton;
            var anim = GetSprite(player.State);
            var index = anim.MapIndex(player.StateFrame);
            var sprite = anim.GetByIndex(index);
            _renderer.sprite = sprite;
            _renderer.color = player.OnDamage ? Color.red : Color.white;
        }
    
        private void LateUpdate()
        {
            _interpolactionIndicator.SetActive(_lastPosition != _curPosition);
            var lerpSpeed = _config != null ? _config.LerpSpeed : 100f;
            var pos = Vector3.Lerp(_lastPosition, _curPosition, lerpSpeed * Time.deltaTime);
            _lastPosition = pos;
            transform.localPosition = pos;
        }

        private FixedAnimation GetSprite(Api.PlayerState state)
        {
            return state switch
            {
                Api.PlayerState.Idle           => _animator.GetAnimation("Idle"),
                Api.PlayerState.Run            => _animator.GetAnimation("Run"),
                Api.PlayerState.Jump           => _animator.GetAnimation("Jump"),
                Api.PlayerState.Falling        => _animator.GetAnimation("Falling"),
                Api.PlayerState.Landing        => _animator.GetAnimation("Hurt"),
                Api.PlayerState.AttackOnGround => _animator.GetAnimation("Attack"),
                Api.PlayerState.Death          => _animator.GetAnimation("Death"),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }

    }
}
