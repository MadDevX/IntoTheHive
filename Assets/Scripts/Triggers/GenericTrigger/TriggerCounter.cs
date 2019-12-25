using Relays;
using Relays.Internal;
using System;
using UnityEngine;
using Zenject;

/// <summary>
/// Count player characters that entered the Trigger contained in triggerCounter.
/// Is Fired Only on host, clients have dummy versions on their side.
/// </summary>
public class HostTriggerCounter: IDisposable
{
    private LivingCharactersRegistry _livingPlayersManager;
    private ITriggerable _triggerHandler;
    private TriggerRelay _relay;
    private int _counter = 0;

    //To be removed
    private ClientInfo _status;

    public HostTriggerCounter(
        LivingCharactersRegistry livingPlayersManager,
        ITriggerable triggerHandler,
        TriggerRelay relay,
        ClientInfo status)
    {
        _triggerHandler = triggerHandler;
        _livingPlayersManager = livingPlayersManager;
        _relay = relay;
        _status = status;
    }

    [Inject]
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
            if (_livingPlayersManager.LivingPlayersCount <= _counter)
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
            _counter--;
        }
    }
}