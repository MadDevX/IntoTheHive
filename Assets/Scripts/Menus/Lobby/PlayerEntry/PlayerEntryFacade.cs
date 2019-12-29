using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerEntryFacade: MonoBehaviour, IPoolable<PlayerEntrySpawnParameters, IMemoryPool>, IDisposable
{
    private Text _nameText;
    private Text _readyStatusText;
    private Text _notReadyStatusText;

    private IMemoryPool _pool;

    public PlayerEntryFacade(
        [Inject(Id =Identifiers.PlayerEntryName)]
        Text nameText,
        [Inject(Id =Identifiers.PlayerEntryReady)]
        Text readyStatusText,
        [Inject(Id =Identifiers.PlayerEntryNotReady)]
        Text notReadyStatusText
        )
    {
        _nameText = nameText;
        _readyStatusText = readyStatusText;
        _notReadyStatusText = notReadyStatusText;
    }
  
    public void OnSpawned(PlayerEntrySpawnParameters parameters, IMemoryPool memoryPool)
    {
        _pool = memoryPool;
        this.SetPlayerName(parameters.PlayerName);
        this.SetReadyStatus(false);
    }

    public void Dispose()
    {
        _pool?.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void SetReadyStatus(bool ready)
    {
        _readyStatusText.enabled = ready;
        _notReadyStatusText.enabled = !ready;
    }

    public void SetPlayerName(string name)
    {
        _nameText.text = name;
    }  

    public class Factory: PlaceholderFactory<PlayerEntrySpawnParameters, PlayerEntryFacade>
    {
    }
}