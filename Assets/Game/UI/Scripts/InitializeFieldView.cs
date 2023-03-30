using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Api;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class InitializeFieldView : MonoBehaviour
{
    [SerializeField] private Color _inputOkColor;
    [SerializeField] private Color _inputErrorColor;
    [SerializeField] private TMP_Dropdown _localOrRemote;
    [SerializeField] private TMP_InputField _localPortFiled;
    [SerializeField] private TMP_InputField _remoteIpField;
    [SerializeField] private Button _connetBtn;

    private bool? _isLocal;
    private ushort? _localPort;
    private ushort? _remotePort;
    private string _ip;

    public event Action<InitArgs> Connect; 

    private void Start()
    {
        LocalOrRemoteValueChanged(_localOrRemote.value);
        LocalPortFieldChanged(_localPortFiled.text);
        RemoteIpFieldChanged(_remoteIpField.text);
        ValidateInput();
        
        _localOrRemote.onValueChanged.AddListener(LocalOrRemoteValueChanged);
        _localPortFiled.onValueChanged.AddListener(LocalPortFieldChanged);
        _remoteIpField.onValueChanged.AddListener(RemoteIpFieldChanged);
        _connetBtn.onClick.AddListener(ConnectPressed);
    }

    private void LocalOrRemoteValueChanged(int arg0)
    {
        _isLocal = arg0 switch
        {
            0 => true,
            1 => false,
            _ => null
        };
        ValidateInput();
    }

    private void LocalPortFieldChanged(string arg0)
    {
        if (ushort.TryParse(arg0, out ushort port))
        {
            _localPortFiled.textComponent.color = _inputOkColor;
            _localPort = port;
        }
        else
        {
            _localPortFiled.textComponent.color = _inputErrorColor;
            _localPort = null;
        }
        ValidateInput();
    }

    private void RemoteIpFieldChanged(string arg0)
    {
        if (TryParseAddress(arg0, out var ip, out var port))
        {
            _remoteIpField.textComponent.color = _inputOkColor;
            _remotePort = port;
            _ip = ip;
        }
        else
        {
            _remoteIpField.textComponent.color = _inputErrorColor;
            _remotePort = null;
            _ip = null;
        }
        ValidateInput();
    }

    private bool ValidateInput()
    {
        bool isValid = _isLocal.HasValue &&
                       _localPort.HasValue &&
                       _remotePort.HasValue &&
                       _ip != null;

        _connetBtn.interactable = isValid;
        return isValid;
    }
    
    private void ConnectPressed()
    {
        var args = new InitArgs
        {
            IsLocal = _isLocal.Value,
            LocalPort = _localPort.Value,
            RemotePort = _remotePort.Value,
            Ip = _ip,
        };
        Connect?.Invoke(args);
    }

    private void OnDestroy()
    {
        Connect = null;
    }

    private static bool TryParseAddress(string addr, out string ip, out ushort port)
    {
        port = 0;
        ip = null;
        
        int separaotr = addr.LastIndexOf(':');
        if (separaotr < 0)
            return false;

        if (!IPAddress.TryParse(addr[..separaotr], out var ipAddr))
            return false;

        if (!ushort.TryParse(addr[(separaotr + 1)..], out port))
            return false;

        ip = ipAddr.ToString();

        return true;
    }
}
