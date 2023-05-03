using System;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class CameraPursuer : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private Vector3 _offset = new Vector3(0, 0, -50f);
        [SerializeField] private float _lerp = 0.5f;

        private Vector2 _position;
        private Vector2 _target;

        public Vector2 Target
        {
            get => _target;
            set
            {
                _target = value;
                Position = Vector2.Lerp(_position, _target, _lerp * Time.deltaTime);
            } 
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _camera.position = (Vector3) _position + _offset;
            }
        }
    }
}