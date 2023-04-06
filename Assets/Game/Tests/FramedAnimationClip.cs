using UnityEngine;


namespace Game.Tests
{
    [CreateAssetMenu(menuName = "Game/Test/FramedAnimationClip")]
    class FramedAnimationClip : ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _time;
        [SerializeField] private bool _repeate;

        public int Count => _sprites.Length;

        public float Time => _time;

        public bool Repeate => _repeate;

        public Sprite Get(int i) => _sprites[Mathf.Abs(i) % _sprites.Length];
    }


}