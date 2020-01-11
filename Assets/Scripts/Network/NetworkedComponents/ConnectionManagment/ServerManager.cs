using DarkRift.Client.Unity;
using DarkRift.Server.Unity;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// This is a ProjectContext class which holds the server created at the beggining of the game.
/// </summary>
public class ServerManager: IDisposable
{
    private XmlUnityServer _server;
    private UnityClient _client;

    public ServerManager(
        XmlUnityServer server,
        UnityClient client)
    {
        _server = server;
        _client = client;
    }
   
    public void CreateServer()
    {
        //_server.Create();
    }

    public void CloseServer()
    {
        // TODO MG is it necessary to disconnect all players manually?
        //_server.Close();
        //Debug.Log("Server is not closing properly");
    }

    /// <summary>
    /// Joins the local server as host without sumbiting the it's IP or port number.
    /// </summary>
    public void JoinAsHost()
    {
        // TODO MG extract this data from configuration file
        IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
        int port = 4296;

        // Unfortunately a lot of code has to battle the following problem: https://github.com/DarkRiftNetworking/DarkRift/issues/81
        if (_client.ConnectionState == DarkRift.ConnectionState.Connecting)
        {
            try
            {
                _client.Disconnect();
            }
            catch (SocketException) { }
        }

        _client.Connect(serverAddress,port,DarkRift.IPVersion.IPv4);
    }

    public void Dispose()
    {
        this.CloseServer();
    }
}