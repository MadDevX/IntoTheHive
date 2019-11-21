using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


/// <summary>
/// Exists Only on ConnectionMenu Screen
/// Is used to initialize client connection to the server
/// </summary>
public class NetworkedClientInitializer
{
    private UnityClient _client;
    private InputField _ipAddressInputField;
    private InputField _portNumberInputField;
    
    private IPAddress address;
    private int port;

    public NetworkedClientInitializer(
        [Inject(Id = Identifiers.ConnetionMenuIpInputField)] InputField ipAddressInputField,
        [Inject(Id = Identifiers.ConnetionMenuPortInputField)] InputField portNumberInputField,
        UnityClient client)
    {
        _ipAddressInputField = ipAddressInputField;
        _portNumberInputField = portNumberInputField;
        _client = client;
    }

    public void JoinServer()
    {
        // button method
        int port;
        IPAddress address;
        bool ipParsed = IPAddress.TryParse(_ipAddressInputField.textComponent.text, out address);
        bool portParsed = int.TryParse(_portNumberInputField.textComponent.text, out port);

        if (ipParsed == false)
        { 
            Debug.Log("Incorrect Ip - jakies okienko");
        }

        if(portParsed == false)
        {
            Debug.Log("Incorrect port - jakies okienko");
        }

        if(ipParsed && portParsed)
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