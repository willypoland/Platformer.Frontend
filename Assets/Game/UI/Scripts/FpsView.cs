using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FpsView : MonoBehaviour
{
    [SerializeField] private Text _text;
    void Update()
    {
        _text.text = (1f / Time.deltaTime).ToString();
    }
}
