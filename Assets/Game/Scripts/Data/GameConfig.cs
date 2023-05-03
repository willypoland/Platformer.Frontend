using UnityEngine;


namespace Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private Color _playerShapeColor = Color.gray;
        [SerializeField] private Color _attackShapeColor = Color.red;
        [SerializeField, Min(0.001f)] private float tickDelta = 0.0166666f;
        [SerializeField] private Vector2 _maxSceneSize = new Vector2(1000f, 1000f);
        [SerializeField] private float _unitScale = 32f;
        [SerializeField] private bool _flipY = true;
        [SerializeField] private Color _platfromMarkerColor = new Color(0.3f, 0.9f, 0.2f, 0.5f);
        [SerializeField] private Color _platfromMarkerInvalidColor = Color.red;
        
        public Color PlatfromGizmoColor => _platfromMarkerColor;
        public Color PlayerShapeColor => _playerShapeColor;
        public Color AttackShapeColor => _attackShapeColor;
        public float TickDelta => tickDelta;
        public bool FlipY => _flipY;
        public float UnitScale => _unitScale;
        public Vector2 MaxSceneSize => _maxSceneSize;
        public Color PlatfromMarkerInvalidColor => _platfromMarkerInvalidColor;
    }
}