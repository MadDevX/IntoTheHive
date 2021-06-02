using UnityEngine;

/// <summary>
/// This class holds all rooms' prefabs
/// </summary>
[CreateAssetMenu(menuName = "GameResources/Triggers")]
public class Triggers : ScriptableObject
{
    [SerializeField] public Object EndLevelTrigger;
    [SerializeField] public Object StartLevelTrigger;    

    public Object GetPrefabByID(int id)
    {
        if (id == 0) return EndLevelTrigger;
        if (id == 1) return StartLevelTrigger;

        return EndLevelTrigger;
    }
}