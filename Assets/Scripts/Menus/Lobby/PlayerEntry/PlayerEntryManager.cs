using System.Collections.Generic;
using Zenject;

public class PlayerEntryManager
{    
    private PlayerEntryFacade.Factory _factory;

    private List<PlayerEntryFacade> _playerEntries = new List<PlayerEntryFacade>();

    public PlayerEntryManager(
        [Inject(Id = Identifiers.PlayerEntryPool)]
        PlayerEntryFacade.Factory factory
        )
    {
        _factory = factory;
    }

    public void SetReady(string name, ushort id, bool ready)
    {
        var facade = _playerEntries.Find(entry => entry.Id == id);
        if (facade != null)
        {
            facade.SetReadyStatus(ready);
        }
        else
        {
            var parameters = new PlayerEntrySpawnParameters
            {
                Id = id,
                PlayerName = name
            };
            var newEntry = _factory.Create(parameters);
            _playerEntries.Add(newEntry);
            newEntry.SetReadyStatus(ready);
        }
    }

    public void RemoveEntry(ushort id)
    {
        var facade = _playerEntries.Find(entry => entry.Id == id);
        if(facade != null)
        {
            facade.Dispose();
            _playerEntries.Remove(facade);
        }
    }

}