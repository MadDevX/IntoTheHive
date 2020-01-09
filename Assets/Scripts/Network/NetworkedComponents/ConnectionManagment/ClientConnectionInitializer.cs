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
        Debug.Log("JoinServerFired");
        int port;
        IPAddress address;
        bool ipParsed = false;
        _errorText.text = "";
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
                Debug.Log(_client.ConnectionState);

                Debug.Log(_client.ConnectionState);
                if (_client.ConnectionState != DarkRift.ConnectionState.Connecting)
                {
                    try
                    {

                        var isConnected = Task.Run(() => _client.Connect(address, port, DarkRift.IPVersion.IPv4)).Wait(3000);
                        if (_client.ConnectionState != DarkRift.ConnectionState.Connected)
                        {
                            _client.Disconnect();
                            _errorText.text = "Couldn't Connect discon";
                        }
                        Debug.Log("isConnected = " + isConnected);
                    }
                    catch (AggregateException e)
                    {
                        Debug.Log(e.Message);
                        _errorText.text = "Agg";
                    }
                    catch (ArgumentException)
                    {
                        _errorText.text = "Incorrect Port Number";
                    }
                    catch (SocketException)
                    {
                        _errorText.text = "Couldn't connect";
                    }

                    if (_client.ConnectionState != DarkRift.ConnectionState.Connected)
                    {
                        // After 3 seconds break the connection
                        //_errorText.text = "Couldn't connect";

                    }

                }
                else
                {

                }
            }
        }
    }
}