using System;
using Zenject;

/// <summary>
/// This class is used to initialize game after changing scenes.
/// Methods from this class are executed when a project wide event is called from the other scene 
/// To make this class scene context based, an inter-scene event had to be introduced to trigger 
/// methods from this class without being bound at the time of a call to change scenes.
/// </summary>
public class GameplayInitializer: IInitializable, IDisposable
{

    private GenericMessageWithResponseHost _messageWithResponse;
    private NetworkedCharacterSpawner _characterSpawner;
    private LevelGraphMessageSender _graphSender;
    private ProjectEventManager _eventManager;

    public GameplayInitializer(
        GenericMessageWithResponseHost messageWithResponse,
        NetworkedCharacterSpawner characterSpawner,
        LevelGraphMessageSender graphSender,
        ProjectEventManager eventManager
        )
    {
        _messageWithResponse = messageWithResponse;
        _characterSpawner = characterSpawner;
        _graphSender = graphSender;
        _eventManager = eventManager;
    }

    public void Initialize()
    {
        _eventManager.GameInitializedHost += LoadLevel;
    }

    public void Dispose()
    {
        _eventManager.GameInitializedHost -= LoadLevel;
    }

    public void LoadLevel()
    {
        _messageWithResponse.SendMessageWithResponse(_graphSender.GenerateLevelGraphMessage(), SpawnPlayers);
    }

    public void SpawnPlayers()
    {
        _messageWithResponse.SendMessageWithResponse(_characterSpawner.GenerateSpawnMessage(), BeginGame);
    }

    public void BeginGame()
    {
        // Probably something like turning on the players' controls
    }
}
