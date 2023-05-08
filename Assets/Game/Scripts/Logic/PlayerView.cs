using UnityEngine;


namespace Game.Scripts.Logic
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private InterpolatedGameObjectView _interpolated;

        public InterpolatedGameObjectView InterpolatedGameObjectView => _interpolated;
        public GameObjectView GameObjectView => _interpolated.GameObjectView;
        public PlayerAnimator Animator => _animator;
    }
}