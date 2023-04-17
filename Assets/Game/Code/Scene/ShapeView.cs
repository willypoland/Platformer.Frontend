using UnityEngine;


namespace Game.Code.Scene
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShapeView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _image;

        private void Reset()
        {
            Start();
        }

        private void Start()
        {
            _image = GetComponent<SpriteRenderer>();
        }

        public void Set(Vector2 leftTop, Vector2 size)
        {
            var center = leftTop + size * 0.5f;

            transform.localPosition = center;
            transform.localScale = size;
        }

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }
    }
}