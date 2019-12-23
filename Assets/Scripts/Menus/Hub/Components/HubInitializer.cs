using System;
using Zenject;

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
        _messageWithResponse.SendMessageWithResponse(_characterSpawner.GenerateSpawnMessage());

    }
}