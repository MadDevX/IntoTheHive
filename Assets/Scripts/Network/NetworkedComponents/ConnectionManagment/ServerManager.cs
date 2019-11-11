using DarkRift.Client.Unity;
using DarkRift.Server.Unity;
using System;
using System.Collections.Generic;
using Zenject;

public class ServerManager:IDisposable
{
    private XmlUnityServer _server;
    private UnityClient _client;
    private Dictionary<ushort, bool> _readiedPlayers;

    public ServerManager(
        XmlUnityServer server,
        UnityClient client)
    {
        _server = server;
        _client = client;
        _readiedPlayers = new Dictionary<ushort, bool>();
    }

    public void CreateServer()
    {
        _server.Create();
    }

    public void CloseServer()
    {
        _server.Close();
    }

    public void Dispose()
    {
        this.CloseServer();
    }
}