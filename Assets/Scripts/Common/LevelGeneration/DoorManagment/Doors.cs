using UnityEngine;

/// <summary>
/// This class holds all doors' prefabs
/// </summary>
[CreateAssetMenu(menuName = "GameResources/Doors")]
public class Doors : ScriptableObject
{
    [SerializeField] public Object BasicDoor;
    [SerializeField] public Object ClosedDoor;
}