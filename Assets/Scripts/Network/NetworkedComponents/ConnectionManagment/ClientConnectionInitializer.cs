using DarkRift.Client.Unity;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static DarkRift.Client.DarkRiftClient;

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
    private Text _buttonText;
    private Button _joinServerButton;
    private Button _createServerButton;
    private Button _backButton;

    public ClientConnectionInitializer(
        [Inject(Id = Identifiers.ConnetionMenuIpInputField)] InputField ipAddressInputField,
        [Inject(Id = Identifiers.ConnetionMenuPortInputField)] InputField portNumberInputField,
        [Inject(Id = Identifiers.ConnetionMenuJoinServerButton)] Button joinServerButton,
        [Inject(Id = Identifiers.ConnetionMenuCreateServerButton)] Button createServerButton,
        [Inject(Id = Identifiers.ConnetionMenuBackButton)] Button backButton,
        UnityClient client,
        Text errorText)
    {
        _ipAddressInputField = ipAddressInputField;
        _portNumberInputField = portNumberInputField;
        _client = client;
        _errorText = errorText;
        _joinServerButton = joinServerButton;
        _createServerButton = createServerButton;
        _backButton = backButton;
        _buttonText = _joinServerButton.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Joins a server with IP and port number specified by the connection menu input fields.
    /// </summary>
    public void JoinServer()
    {
        _errorText.text = "";
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

    /// <summary>
    /// Connects with a given Ip address on a given port. Disables buttons until the connection times out or connects.
    /// </summary>
    /// <param name="address"> IP address of the server </param>
    /// <param name="port"> Port of the server </param>
    private async void Connect(IPAddress address, int port)
    {
        // Unfortunately a lot of code has to battle the following problem: https://github.com/DarkRiftNetworking/DarkRift/issues/81
        if (_client.ConnectionState == DarkRift.ConnectionState.Connecting)
        {
            try
            {
                _client.Disconnect();
            }
            catch (SocketException) { }
        }

        SwitchButtonsInteractable(false);
        _buttonText.text = "Connecting...";

        await Task.Run(() => _client.ConnectInBackground(address, port, DarkRift.IPVersion.IPv4, (Exception e) => { HandleConnectionCompleted(e); }));
        
    }

    /// <summary>
    /// Handles completion of ConnectInBackground method from DarkRift
    /// </summary>
    /// <param name="e">Exception that occured, if any.</param>
    private void HandleConnectionCompleted(Exception e)
    {
        if(e != null)        
        {
            HandleConnectionException(e);
        }

        _buttonText.text = "Join server";
        SwitchButtonsInteractable(true);
    }

    /// <summary>
    /// Handle expception thrown during connection.
    /// </summary>
    /// <param name="e">Exception that was thrown.</param>
    private void HandleConnectionException(Exception e)
    {
        switch (e)
        {
            case ArgumentException arg:
                _errorText.text = "Incorrect Port Number";
                break;
            case SocketException socket:
                _errorText.text = "Couldn't connect";
                break;
        }
    }

    /// <summary>
    /// Makes the buttons interactable or not.
    /// </summary>
    /// <param name="isActive">Whether the buttons should be active.</param>
    private void SwitchButtonsInteractable(bool isActive)
    {
        _joinServerButton.interactable = isActive;
        _createServerButton.interactable = isActive;
        _backButton.interactable = isActive;
    }
}