using System.Collections;
using System.Collections.Generic;
using Api;
using UnityEngine;
using UnityEngine.UI;


public class InputMapView : MonoBehaviour
{
    [SerializeField] private Color _default = Color.gray;
    [SerializeField] private Color _press = Color.green;
    [SerializeField] private Image _up;
    [SerializeField] private Image _down;
    [SerializeField] private Image _left;
    [SerializeField] private Image _right;
    [SerializeField] private Image _lmb;

    public void Set(InputMap map)
    {
        
        _up.color = GetColor(map.UpPressed);
        _down.color = GetColor(map.DownPressed);
        _left.color = GetColor(map.LeftPressed);
        _right.color = GetColor(map.RightPressed);
        _lmb.color = GetColor(map.LeftMouseClicked);
    }

    private Color GetColor(bool press) => press ? _press : _default;
}
