using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsInterpolationTest : MonoBehaviour
{
    [SerializeField] private float _speed = 6;
    [SerializeField] private float _raySacle = 1f;
    
    private Rigidbody2D _rigidbody;
    private bool _updated;

    private Vector3 _prevPos;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _prevPos = transform.position;
    }

    private void Update()
    {
        var pos = transform.position;
        
        _rigidbody.velocity = GetMoveDirection() * _speed;

        var speed = (pos - _prevPos).magnitude;

        var color = _updated ? Color.magenta : Color.cyan;
        _updated = false;
        Debug.DrawRay(_prevPos, Vector3.up * speed * _raySacle, color, 1f);
        
        

        _prevPos = pos;
        _updated = false;
    }

    private void FixedUpdate()
    {
        _updated = true;
    }

    private Vector2 GetMoveDirection()
    {
        return new Vector2(GetAxis(KeyCode.D, KeyCode.A), GetAxis(KeyCode.W, KeyCode.S)).normalized;
    }

    private float GetAxis(KeyCode positive, KeyCode negative)
    {
        float x = 0;
        
        if (Input.GetKey(positive))
            x += 1;

        if (Input.GetKey(negative))
            x -= 1;
        
        return x;
    }
}
