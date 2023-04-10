using System;
using UnityEngine;

namespace Game
{
    public class InitFieldViewShowButton : MonoBehaviour
    {
        [SerializeField] private KeyCode _hotkey = KeyCode.Tab;
        [SerializeField] private GameObject _target;
        [SerializeField] private GameObject _thenShow;
        [SerializeField] private GameObject _thenHide;
        [SerializeField] private bool _showed = true;

        private void Start()
        {
            ApplyShowedState();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_hotkey))
                Toggle();
        }

        private void Show()
        {
            _target.SetActive(true);
            _thenShow.SetActive(true);
            _thenHide.SetActive(false);
        }

        private void Hide()
        {
            _target.SetActive(false);
            _thenShow.SetActive(false);
            _thenHide.SetActive(true);
        }

        private void ApplyShowedState()
        {
            if (_showed)
                Show();
            else
                Hide();
        }

        public void Toggle()
        {
            _showed = !_showed;
            ApplyShowedState();
        }
    }
}