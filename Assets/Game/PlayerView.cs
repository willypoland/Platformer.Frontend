using Game;
using UnityEngine;


public class PlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _attackFrames = 6;

    private int _attackComboCount = 0;

    public void Set(Ser.Player data, ShapeMock shape)
    {
        transform.localPosition = shape.transform.localPosition;

        _renderer.flipX = data.LeftDirection;

        if (data.StateFrame == 0)
        {
            PlayAnimation(data.State);
        }
    }

    private void PlayAnimation(Ser.PlayerState state)
    {
        switch (state)
        {
        case Ser.PlayerState.Idle:
            _animator.Play("Idle");
            break;
        case Ser.PlayerState.Run:
            _animator.Play("Run");
            break;
        case Ser.PlayerState.Jump:
            _animator.Play("Jump");
            break;
        case Ser.PlayerState.Falling:
            _animator.Play("Fall");
            break;
        case Ser.PlayerState.Landing:
            _animator.Play("Hurt");
            break;
        case Ser.PlayerState.AttackOnGround:
            _animator.PlayInFixedTime("Attack" + (_attackComboCount + 2), 0, 1f / _attackFrames);
            _attackComboCount = (_attackComboCount + 1) % 2;
            break;
        case Ser.PlayerState.Death:
            _animator.Play("Death");
            break;
        default:
            _animator.Play("Idle");
            break;
        }
    }
}
