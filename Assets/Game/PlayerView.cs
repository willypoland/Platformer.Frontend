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

    private int _prevFrame;
    private Ser.PlayerState _prevState;
    private bool _wasRollback;
    private Vector3 _prevPosition = new Vector3(float.NegativeInfinity, 0, 0);

    public void Set(Ser.Player data, ShapeMock shape, bool wasRollback)
    {
        var newPos = shape.transform.localPosition;
        wasRollback = IsWasRollback(data);
        newPos = InterpolateAfterRollback(wasRollback, newPos);
        
        if (wasRollback)
            Debug.Log("was rollback");
        
        transform.localPosition = newPos;
        _renderer.flipX = data.LeftDirection;
        var anim = GetSprite(data.State);
        var sprite = anim.Get(data.StateFrame);
        _renderer.sprite = sprite;
        _renderer.color = data.OnDamage ? Color.red : Color.white;
        _prevPosition = newPos;
        _stateView.Set(data, transform.position, _renderer.sprite, anim);
    }

    private Vector3 InterpolateAfterRollback(bool wasRollback, Vector3 newPos)
    {
        if (float.IsInfinity(_prevPosition.x))
            _prevPosition = newPos;

        _wasRollback |= wasRollback;

        _interpolactionIndicator.SetActive(_wasRollback);

        if (_wasRollback)
        {
            newPos = Vector3.Lerp(_prevPosition, newPos, _lerpSpeed * Time.deltaTime);
            if ((_prevPosition - newPos).sqrMagnitude < _lerpThreshold)
                _wasRollback = false;
        }

        return newPos;
    }

    private bool IsWasRollback(Ser.Player data)
    {
        var curFrame = data.StateFrame;
        var curState = data.State;

        bool wasRoolback = (_prevState == curState) && curFrame - _prevFrame != 1;
        
        _prevFrame = curFrame;
        _prevState = curState;

        return wasRoolback;
    }

    // private Sprite GetSprite(Ser.Player data)
    // {
    //     return data.State switch
    //     {
    //         Ser.PlayerState.Idle           => _animator.GetSprite("Idle", data.StateFrame),
    //         Ser.PlayerState.Run            => _animator.GetSprite("Run", data.StateFrame),
    //         Ser.PlayerState.Jump           => _animator.GetSprite("Jump", data.StateFrame),
    //         Ser.PlayerState.Falling        => _animator.GetSprite("Falling", data.StateFrame),
    //         Ser.PlayerState.Landing        => _animator.GetSprite("Hurt", data.StateFrame),
    //         Ser.PlayerState.AttackOnGround => _animator.GetSprite("Attack", data.StateFrame),
    //         Ser.PlayerState.Death          => _animator.GetSprite("Death", data.StateFrame),
    //         _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
    //     };
    // }
    
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
