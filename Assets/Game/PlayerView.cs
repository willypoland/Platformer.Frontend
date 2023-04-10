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

    private bool _wasRollback;
    private Vector3 _prevPosition = new Vector3(float.NegativeInfinity, 0, 0);

    public void Set(Ser.Player data, ShapeMock shape, bool wasRollback)
    {
        var newPos = shape.transform.localPosition;

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

        transform.localPosition = newPos;

        _renderer.flipX = data.LeftDirection;
        
        _renderer.sprite = GetSprite(data);

        _renderer.color = data.OnDamage ? Color.red : Color.white;

        _prevPosition = newPos;
    }

    private Sprite GetSprite(Ser.Player data)
    {
        return data.State switch
        {
            Ser.PlayerState.Idle           => _animator.GetSprite("FAnim Idle", data.StateFrame),
            Ser.PlayerState.Run            => _animator.GetSprite("FAnim Run", data.StateFrame),
            Ser.PlayerState.Jump           => _animator.GetSprite("FAnim Jump", data.StateFrame),
            Ser.PlayerState.Falling        => _animator.GetSprite("FAnim Falling", data.StateFrame),
            Ser.PlayerState.Landing        => _animator.GetSprite("FAnim Hurt", data.StateFrame),
            Ser.PlayerState.AttackOnGround => _animator.GetSprite("FAnim Attack", data.StateFrame),
            Ser.PlayerState.Death          => _animator.GetSprite("FAnim Death", data.StateFrame),
            _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
        };
    }
}
