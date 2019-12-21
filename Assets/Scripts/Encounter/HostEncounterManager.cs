/// <summary>
/// Handles the flow of the encounter. Begins it reacting to the RoomEnteredTrigger and end when all enemies are dead.
/// </summary>
public class HostEncounterManager
{
    private GenericMessageWithResponseHost _messageWithResponse;
    private HostDoorManager _doorManager;
    // private HostEncouterEnemyManager - spawns enemies and invokes AllwavesClearedEvent
    // 

    // TEMPORARY
    private NetworkedAISpawner _aISpawner;
    // TEMPORARY

    private int _encounterID = -1;

    public HostEncounterManager(
        NetworkedAISpawner aISpawner,

        GenericMessageWithResponseHost messageWithResponse,
        HostDoorManager doorManager)
    {
        _aISpawner = aISpawner;

        _messageWithResponse = messageWithResponse;
        _doorManager = doorManager;
    }

    public void BeginEncounter(ushort doorId)
    {
        _encounterID = doorId;
        // mark room as visited
        // spawn doors 
        SpawnDoors();
    }

    public void SpawnDoors()
    {
        _messageWithResponse.SendMessageWithResponse(_doorManager.PrepareCloseDoorsMessage((ushort)_encounterID), SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        //_messageWithResponse.SendMessageWithResponse(_aISpawner.GenerateSpawnMessage(0, 10));
    }

    public void EndEncounter()
    {
        _messageWithResponse.SendMessageWithResponse(_doorManager.PrepareOpenDoorsMessage((ushort)_encounterID));
    }

}