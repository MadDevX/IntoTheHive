using DarkRift;
using DarkRift.Client.Unity;
using System;
using UnityEngine;
using Zenject;

public class ClientInfo: IInitializable, IDisposable
{
    // TODO MG: UnityClient is probably unnecessary here
    public UnityClient Client;
    public ushort Status = ClientStatus.None;

    public event Action<ushort> StatusChanged;

    private NetworkRelay _relay;

    public ClientInfo(
        NetworkRelay relay,
        UnityClient client)
    {
        Client = client;
        _relay = relay;
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.ConnectionInfo, HandleConnectionInfo);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.ConnectionInfo, HandleConnectionInfo);
    }

    public void ResetState()
    {
        Status = ClientStatus.None;
    }

    private void HandleConnectionInfo(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            // check message size
            ushort status = reader.ReadUInt16();

            if(Status == ClientStatus.None)
            {
                Status = status;
                StatusChanged?.Invoke(Status);
            }
            
        }
    }

}