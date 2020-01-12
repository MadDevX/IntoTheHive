using DarkRift;
using DarkRift.Client.Unity;
using System;
using UnityEngine;
using Zenject;

public class ClientInfo: IInitializable, IDisposable
{
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

    public void SubscribeOnStatusChanged(Action<ushort> handler)
    {
        ClearSubscribedHandlers();
        StatusChanged += handler;
    }

    public void UnsubscribeOnStatusChanged(Action<ushort> handler)
    {
        var invocationList = StatusChanged?.GetInvocationList();
        if (StatusChanged?.GetInvocationList() != null)
            foreach (var function in invocationList)
            {
                if(handler == (Action<ushort>)function)
                {
                    StatusChanged -= handler;
                 }
            }
    }

    private void ClearSubscribedHandlers()
    {        
        if(StatusChanged?.GetInvocationList() != null)
        {
            if(StatusChanged.GetInvocationList().Length != 0)        
                foreach (var handler in StatusChanged.GetInvocationList())
                {
                    StatusChanged -= (Action<ushort>)handler;
                }
        }
    }

    private void HandleConnectionInfo(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            ushort status = reader.ReadUInt16();

            if(Status == ClientStatus.None)
            {
                Status = status;
                StatusChanged?.Invoke(Status);
            }
            
        }
    }

}