using Relays;
using Relays.Internal;
using System;
using UnityEngine;
using Zenject;

/// <summary>
/// Count player characters that entered the Trigger contained in triggerCounter.
/// Is Fired Only on host, clients have dummy versions on their side.
/// </summary>
public class HostTriggerCounter: IInitializable, IDisposable
{
    private GlobalHostPlayerManager _playerManager;
    private ITriggerable _triggerHandler;
    private BoxCollider2D _trigger;
    private TriggerRelay _relay;
    private int _counter = 0;

    //To be removed
    private ClientInfo _status;

    public HostTriggerCounter(
        GlobalHostPlayerManager playerManager,
        ITriggerable triggerHandler,
        BoxCollider2D trigger,
        TriggerRelay relay,
        ClientInfo status)
    {
        _triggerHandler = triggerHandler;
        _playerManager = playerManager;
        _trigger = trigger;
        _relay = relay;
        _status = status;
    }

    // However it works in job project so
    // Not IInitlizable because this class is created dynamically throught the factory
    
    public void Initialize()
    {
        _relay.OnTrigger2DEnterEvt += HandleTriggerEnter;
        _relay.OnTrigger2DExitEvt += HandleTriggerExit;
    }

    public void Dispose()
    {
        _relay.OnTrigger2DEnterEvt -= HandleTriggerEnter;
        _relay.OnTrigger2DExitEvt -= HandleTriggerExit;
    }

    private void HandleTriggerEnter(Collider2D obj)
    {
        var playerFacade = obj.GetComponent<CharacterFacade>();
        if(playerFacade != null && playerFacade.CharacterType == CharacterType.Player)
        {
            _counter++;
            Debug.Log(_counter);
            //TODO MG : this line is not affected by disconnects! look out!
            Debug.Log(_playerManager.ConnectedPlayers.Count);

            if (_playerManager.ConnectedPlayers.Count <= _counter)
            {
                //TODO MG REMOVE THIS IF AND SPAWN triggers with logic only on the host side
                if (_status.Status == ClientStatus.Host)
                    _triggerHandler.Trigger();
            }
        }
    }

    private void HandleTriggerExit(Collider2D obj)
    {
        var playerFacade = obj.GetComponent<CharacterFacade>();
        if (playerFacade!= null && playerFacade.CharacterType == CharacterType.Player)
        {
            Debug.Log(_counter);
            _counter--;
        }
    }
}