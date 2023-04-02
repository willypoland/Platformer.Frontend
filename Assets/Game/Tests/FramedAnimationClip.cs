using UnityEngine;


namespace Game.Tests
{
    [CreateAssetMenu(menuName = "Game/Test/FramedAnimationClip")]
    class FramedAnimationClip : ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;

        public Sprite Get(int i) => _sprites[Mathf.Abs(i) % _sprites.Length];

        public int Count => _sprites.Length;

        public Sprite GetLerp(int i, int last) => Get(Mathf.RoundToInt(i / (float) last * Count));
    }
}