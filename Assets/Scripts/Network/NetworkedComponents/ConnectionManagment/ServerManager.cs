using DarkRift.Client.Unity;
using DarkRift.Server.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerManager: IDisposable
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
        // TODO MG is it necessary to disconnect all players manually?
        _server.Close();
        Debug.Log("Server is not closing properly");
    }

    public void JoinAsHost()
    {
        // TODO MG extract this data from configuration file
        IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
        int port = 4296;
        _client.Connect(serverAddress,port,DarkRift.IPVersion.IPv4);
    }

    public void Dispose()
    {
        this.CloseServer();
    }
}