using UnityEngine;
/// <summary>
/// Handles the flow of the encounter. Begins it reacting to the RoomEnteredTrigger and end when all enemies are dead.
/// </summary>
public class HostEncounterManager
{
    private HostEncounterEnemyManager _encounterEnemyManager;
    private GenericMessageWithResponseHost _messageWithResponse;
    private HostDoorManager _doorManager;
    
    private RoomFacade _currentRoom = null;

    public HostEncounterManager(
        HostEncounterEnemyManager encounterEemyManager,
        GenericMessageWithResponseHost messageWithResponse,
        HostDoorManager doorManager)
    {
        _encounterEnemyManager = encounterEemyManager;
        _messageWithResponse = messageWithResponse;
        _doorManager = doorManager;
    }

    public void BeginEncounter(RoomFacade room)
    {
        if (room.Visited == false)
        { 
            room.Visited = true;
            _currentRoom = room;
            _encounterEnemyManager.AllEnemiesDead += EndEncounter;
            SpawnDoors();
        }
    }

    public void SpawnDoors()
    {
        _messageWithResponse.SendMessageWithResponse(_doorManager.PrepareCloseDoorsMessage(_currentRoom.ID), SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        _encounterEnemyManager.SpawnEnemies();
    }

    private void EndEncounter()
    {
        _encounterEnemyManager.AllEnemiesDead -= EndEncounter;
        _messageWithResponse.SendMessageWithResponse(_doorManager.PrepareOpenDoorsMessage(_currentRoom.ID));
    }

}