using DarkRift.Client.Unity;
using System.Net;
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
    
    public ClientConnectionInitializer(
        [Inject(Id = Identifiers.ConnetionMenuIpInputField)] InputField ipAddressInputField,
        [Inject(Id = Identifiers.ConnetionMenuPortInputField)] InputField portNumberInputField,
        UnityClient client)
    {
        _ipAddressInputField = ipAddressInputField;
        _portNumberInputField = portNumberInputField;
        _client = client;
    }

    /// <summary>
    /// Joins a server with IP and port number specified by the connection menu input fields.
    /// </summary>
    public void JoinServer()
    {
        int port;
        IPAddress address;

        bool ipParsed = IPAddress.TryParse(_ipAddressInputField.textComponent.text, out address);
        bool portParsed = int.TryParse(_portNumberInputField.textComponent.text, out port);

        if (ipParsed == false)
        { 
            Debug.Log("Incorrect Ip - jakies okienko");
            // TODO MG : add a modal error window
        }

        if(portParsed == false)
        {
            Debug.Log("Incorrect port - jakies okienko");
            // TODO MG : add a modal error window
        }

        if (ipParsed && portParsed)
        {

            if (_client.ConnectionState != DarkRift.ConnectionState.Connecting)
            {
                _client.Connect(address, port, DarkRift.IPVersion.IPv4);
                // Show popup window which closes on cdisconnect or connected
            }
            // TODO MG do sth if the client didnt connect
        }
    }
}