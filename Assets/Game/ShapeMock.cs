using System;
using Common.Pool;
using UnityEngine;
using UnityEngine.UI;


namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShapeMock : MonoBehaviour
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

        public void Set(int x, int y, int width, int height)
        {
            var leftTop = new Vector3(x, y);
            var size = new Vector3(width, height);
            var center = leftTop + size * 0.5f;

            transform.localPosition = center;
            transform.localScale = size;
        }

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        public class Factory : IPoolObjectFactory<ShapeMock>
        {
            public ShapeMock Source;
            public Transform Root;

            public Factory(Transform root, ShapeMock source)
            {
                Root = root;
                Source = source;
            }

            public ShapeMock Create()
            {
                var newItem = Instantiate(Source, Root);
                newItem.gameObject.SetActive(false);
                return newItem;
            }

            public void ActionOnGet(ShapeMock obj) => obj.gameObject.SetActive(true);

            public void ActionOnRelease(ShapeMock obj) => obj.gameObject.SetActive(false);

            public void ActionOnDispose(ShapeMock obj) => Destroy(obj);
        }
    }
}