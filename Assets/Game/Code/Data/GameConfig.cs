using UnityEngine;


namespace Game.Code.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private Color _platformShapeColor = Color.yellow;
        [SerializeField] private Color _playerShapeColor = Color.gray;
        [SerializeField] private Color _attackShapeColor = Color.red;
        [SerializeField] private float _lerpSpeed = 30f;
        [SerializeField] private float _uintScale = 32f;
        
        public Color PlatformShapeColor => _platformShapeColor;
        public Color PlayerShapeColor => _playerShapeColor;
        public Color AttackShapeColor => _attackShapeColor;
        public float LerpSpeed => _lerpSpeed;
        public float UnitScale => _uintScale;
    }
}