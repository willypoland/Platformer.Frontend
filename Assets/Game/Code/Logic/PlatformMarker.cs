using System;
using Game.Code.Data;
using Game.Code.Infrastructure;
using UnityEngine;


namespace Game
{
    public class PlatformMarker : MonoBehaviour
    {
        [SerializeField] private Rect _rect;
        [SerializeField] private int _type = 0;

        private void Reset()
        {
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0.3f, 0.9f, 0.2f, 0.5f);

            var size = new Vector3(_rect.width, _rect.height);
            Debug.Log(size);
            Gizmos.DrawWireCube(_rect.center, size);
        }

        public Platform GetData(int id, float scale)
        {
            return new Platform()
            {
                
            };
        }
    }
}