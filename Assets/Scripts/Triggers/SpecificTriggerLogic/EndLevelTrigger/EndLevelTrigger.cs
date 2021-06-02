using UnityEngine;
/// <summary>
/// This class handles trigger invocation from TriggerCounter.
/// </summary>
public class EndLevelTrigger : ITriggerable
{
    private WinManager _winManager;

    public EndLevelTrigger(
        WinManager winManager)
    {
        _winManager = winManager;
    }

    public void Trigger()
    {
        _winManager.IncreaseCounter();
    }
}