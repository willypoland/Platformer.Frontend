using System;
using Game.Code.Data;
using Game.Code.UI.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Code.UI
{

    public class ConnectionSetupWindow : MonoBehaviour, IConnectionSetupView
    {
        [SerializeField] private Toggle _toggleLocal;
        [SerializeField] private TMP_InputField _localPortField;
        [SerializeField] private TMP_InputField _remoteAddressField;
        [SerializeField] private Button _connectionButton;
        
        [SerializeField] private Color _invalidInputColor = Color.red;
        [SerializeField] private Color _validInputColor = Color.white;

        public event Func<InputConnectionArguments, InputConnectionArguments> InputChanged;
        public event Action ClickConnection;

        private InputConnectionArguments _arguments = new();

        public void SetArguments(InputConnectionArguments arguments)
        {
            _arguments = arguments;
            Validation();
        }
        
        private void Awake()
        {
            _arguments = new InputConnectionArguments();
            _arguments.LocalPort.Field = "";
            _arguments.RemoteAddress.Field = "";
            
            _toggleLocal.onValueChanged.AddListener(x => _arguments.IsMaster = x);
            _toggleLocal.onValueChanged.AddListener(_ => Validation());
            
            _localPortField.onValueChanged.AddListener(x => _arguments.LocalPort.Field = x);
            _localPortField.onValueChanged.AddListener(_ => Validation());
            
            _remoteAddressField.onValueChanged.AddListener(x => _arguments.RemoteAddress.Field = x);
            _remoteAddressField.onValueChanged.AddListener(_ => Validation());
            
            _connectionButton.onClick.AddListener(OnClickConnection);
        }

        private void OnDestroy()
        {
            InputChanged = null;
            ClickConnection = null;
        }

        private void Validation()
        {
            _arguments.LocalPort.Message = null;
            _arguments.RemoteAddress.Message = null;
            _arguments.LocalPort.IsValid = true;
            _arguments.RemoteAddress.IsValid = true;
            var result = InputChanged?.Invoke(_arguments);
            MapResultFields(result);
        }

        private void OnClickConnection()
        {
            ClickConnection?.Invoke();
        }

        private void MapResultFields(InputConnectionArguments arguments)
        {
            _toggleLocal.isOn = arguments.IsMaster;
            SetupColor(_localPortField, arguments.LocalPort);
            SetupColor(_remoteAddressField, arguments.RemoteAddress);
            _connectionButton.interactable = arguments.LocalPort.IsValid && arguments.RemoteAddress.IsValid;
        }

        private void SetupColor(TMP_InputField input, ValidatedField<string> field)
        {
            input.textComponent.color = field.IsValid ? _validInputColor : _invalidInputColor;
            if (field.IsValid)
                input.SetTextWithoutNotify(field.Field);
        }
    }

}