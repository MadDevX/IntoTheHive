using DarkRift.Server.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ServerInfoTracker : IInitializable
{
    private ClientInfo _info;
    private XmlUnityServer _server;
    private Text _ipText;
    private Text _portText;
    private Settings _settings;

    public ServerInfoTracker(
        ClientInfo info,
        XmlUnityServer server,
        [Inject(Id = Identifiers.IP)]Text ipText,
        [Inject(Id = Identifiers.Port)]Text portText,
        Settings settings)
    {
        _info = info;
        _server = server;
        _ipText = ipText;
        _portText = portText;
        _settings = settings;
    }

    public void Initialize()
    {
        if (_info.Status == ClientStatus.Host)
        {
            _ipText.text = _settings.ipHeader + GetLocalIPAddress();
            _portText.text = _settings.portHeader + _server.Server.ClientManager.Port.ToString();
        }
        else if (_info.Status == ClientStatus.Client)
        {
            _ipText.text = _settings.ipHeader + _info.Client.Address.ToString();
            _portText.text = _settings.portHeader + _info.Client.Port.ToString();
        }
    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    [System.Serializable]
    public class Settings
    {
        public string ipHeader;
        public string portHeader;
    }
}
