using System;
using Game;
using UnityEngine;


public class PlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private FixedAnimationController _animator;
    [SerializeField, Min(1)] private float _lerpThreshold = 1f;
    [SerializeField] private float _lerpSpeed = 30f;
    [SerializeField] private GameObject _interpolactionIndicator;
    [SerializeField] private PlayerStateView _stateView;

    private Ser.PlayerState _prevState;
    private bool _wasRollback;
    private Vector3 _curPosition;
    private Vector3 _lastPosition;

    public void Set(Ser.Player data, ShapeMock shape)
    {
        _lastPosition = _curPosition;
        _curPosition = shape.transform.localPosition;

        // sprite
        _renderer.flipX = data.LeftDirection;
        var anim = GetSprite(data.State);
        var index = anim.MapIndex(data.StateFrame);
        var sprite = anim.GetByIndex(index);
        _renderer.sprite = sprite;
        _renderer.color = data.OnDamage ? Color.red : Color.white;
        
        // views
        _stateView.Set(data, transform.position, sprite, anim, index);
    }

    private void LateUpdate()
    {
        _interpolactionIndicator.SetActive(_lastPosition != _curPosition);
        var pos = Vector3.Lerp(_lastPosition, _curPosition, _lerpSpeed * Time.deltaTime);
        _lastPosition = pos;
        transform.localPosition = pos;
    }

    private FixedAnimation GetSprite(Ser.PlayerState state)
    {
        return state switch
        {
            Ser.PlayerState.Idle           => _animator.GetAnimation("Idle"),
            Ser.PlayerState.Run            => _animator.GetAnimation("Run"),
            Ser.PlayerState.Jump           => _animator.GetAnimation("Jump"),
            Ser.PlayerState.Falling        => _animator.GetAnimation("Falling"),
            Ser.PlayerState.Landing        => _animator.GetAnimation("Hurt"),
            Ser.PlayerState.AttackOnGround => _animator.GetAnimation("Attack"),
            Ser.PlayerState.Death          => _animator.GetAnimation("Death"),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

}
