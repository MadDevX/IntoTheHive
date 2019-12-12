using UnityEngine;
/// <summary>
/// This class handles trigger invocation from TriggerCounter.
/// </summary>
public class EndLevelTrigger : ITriggerable
{
    private HostSceneManager _sceneManager;

    public EndLevelTrigger(
        HostSceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }

    public void Trigger()
    {
        Debug.Log("End Level trigger");
        
        _sceneManager.LoadNextLevel();
    }
}