using DarkRift.Client.Unity;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// ConnectionMenu context only class 
/// Used to initialize client connection to the server.
/// </summary>
public class ClientConnectionInitializer
{
    private UnityClient _client;
    private InputField _ipAddressInputField;
    private InputField _portNumberInputField;
    private Text _errorText;

    public ClientConnectionInitializer(
        [Inject(Id = Identifiers.ConnetionMenuIpInputField)] InputField ipAddressInputField,
        [Inject(Id = Identifiers.ConnetionMenuPortInputField)] InputField portNumberInputField,
        UnityClient client,
        Text errorText)
    {
        _ipAddressInputField = ipAddressInputField;
        _portNumberInputField = portNumberInputField;
        _client = client;
        _errorText = errorText;
    }

    /// <summary>
    /// Joins a server with IP and port number specified by the connection menu input fields.
    /// </summary>
    public void JoinServer()
    {
        
        int port;
        IPAddress address;
        bool ipParsed = false;

        if (_ipAddressInputField.textComponent.text.ToLower().Equals("localhost"))
        {
            address = IPAddress.Parse("127.0.0.1");
            ipParsed = true;
        }
        else
        {
            ipParsed = IPAddress.TryParse(_ipAddressInputField.textComponent.text, out address);
        }

        bool portParsed = int.TryParse(_portNumberInputField.textComponent.text, out port);

        if (ipParsed == false)
        {
             _errorText.text = "Incorrect IP Address";
        }
        else
        {
            if (portParsed == false)
            {
                _errorText.text = "Incorrect Port Number";
            }
            else
            {
                Connect(address, port);               
            }
        }
    }

    private void Connect(IPAddress address, int port)
    {
        if (_client.ConnectionState != DarkRift.ConnectionState.Connecting)
        {
            try
            {
                var isConnected = Task.Run(() => _client.Connect(address, port, DarkRift.IPVersion.IPv4)).Wait(3000);
                if(!isConnected) _errorText.text = "Couldn't connect";
            }
            catch (AggregateException e)
            {
                _errorText.text = "Couldn't connect";
                switch (e.InnerException)
                {
                    case ArgumentException arg:
                        _errorText.text = "Incorrect Port Number";
                        break;
                    case SocketException socket:
                        _errorText.text = "Couldn't connect";
                        break;
                }
            }
        }
        else
        {
            try
            {
                _client.Disconnect();
            }
            catch (SocketException) { }
            _errorText.text = "Try Again Later";
        }
    }
}