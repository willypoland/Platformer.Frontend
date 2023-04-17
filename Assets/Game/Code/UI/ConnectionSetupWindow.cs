using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Code.UI
{
    public class ConnectionSetupWindow : MonoBehaviour, IConnectionSetupWindow
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
            _toggleLocal.onValueChanged.AddListener(x => _arguments.IsMaster = x);
            _toggleLocal.onValueChanged.AddListener(_ => Validation());
            
            _localPortField.onValueChanged.AddListener(x => _arguments.LocalPort = x);
            _localPortField.onValueChanged.AddListener(_ => Validation());
            
            _remoteAddressField.onValueChanged.AddListener(x => _arguments.RemoteAddress = x);
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
            var result = InputChanged?.Invoke(_arguments);
            
            MapResultFields(result);
            
            if (result != null)
                _arguments = result;
        }

        private void OnClickConnection()
        {
            ClickConnection?.Invoke();
        }

        private void MapResultFields(InputConnectionArguments arguments)
        {
            bool anyError = false;

            anyError |= !ValidateField(arguments?.LocalPort, _localPortField.textComponent);
            anyError |= !ValidateField(arguments?.RemoteAddress, _remoteAddressField.textComponent);

            _connectionButton.enabled = anyError;
        }

        private bool ValidateField(string param, Graphic graphic)
        {
            bool result = param != null;
            graphic.color = result ? _validInputColor : _invalidInputColor;
            return result;
        }
    }

}