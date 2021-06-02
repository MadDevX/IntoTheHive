using System;
using Zenject;

/// <summary>
/// Handles host specific logic that happens after the scene is initialized
/// Reply to change scene message takes place in post initialization and this class reacts to event later event.
/// </summary>
public class HubInitializer : IInitializable, IDisposable
{
    private ProjectEventManager _eventManager;
    private NetworkedCharacterSpawner _characterSpawner;
    private GenericMessageWithResponseHost _messageWithResponse;

    public HubInitializer(
        ProjectEventManager eventManager,
        NetworkedCharacterSpawner characterSpawner,
        GenericMessageWithResponseHost messageWithResponse
        )
    {
        _eventManager = eventManager;
        _characterSpawner = characterSpawner;
        _messageWithResponse = messageWithResponse;
    }

    public void Initialize()
    {
        _eventManager.GameInitializedHost += LoadLevel;
    }

    public void Dispose()
    {
        _eventManager.GameInitializedHost -= LoadLevel;
    }

    private void LoadLevel()
    {
        using(var message = _characterSpawner.GenerateSpawnMessage())
        {
            _messageWithResponse.SendMessageWithResponse(message);
        }

    }
}