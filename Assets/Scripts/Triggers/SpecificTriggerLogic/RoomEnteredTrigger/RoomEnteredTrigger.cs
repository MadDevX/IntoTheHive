using UnityEngine;
/// <summary>
/// Begins an encounter when triggered.
/// </summary>
public class RoomEnteredTrigger : ITriggerable
{
    private HostEncounterManager _encounterManager;
    private RoomFacade _facade;

    public RoomEnteredTrigger(
        HostEncounterManager encounterManager,
        RoomFacade facade)
    {
        _facade = facade;
        _encounterManager = encounterManager;
    }

    public void Trigger()
    {
        _encounterManager.BeginEncounter(_facade);
    }
}